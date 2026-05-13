import json
import os
import subprocess
import urllib.parse
import urllib.request
import xml.etree.ElementTree as ET

SONAR_HOST = "https://sonarcloud.io"
TRX_NS = {"trx": "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}


def api_get(path: str, params: dict[str, str], token: str) -> dict:
    query = urllib.parse.urlencode({k: v for k, v in params.items() if v})
    url = f"{SONAR_HOST}{path}?{query}" if query else f"{SONAR_HOST}{path}"
    req = urllib.request.Request(url)
    auth = (token + ":").encode("utf-8")
    import base64

    req.add_header("Authorization", f"Basic {base64.b64encode(auth).decode('ascii')}")

    with urllib.request.urlopen(req) as resp:
        return json.loads(resp.read().decode("utf-8"))


def issues_total(token: str, project_key: str, pr_number: str, tag: str) -> int:
    data = api_get(
        "/api/issues/search",
        {
            "componentKeys": project_key,
            "pullRequest": pr_number,
            "types": "CODE_SMELL",
            "tags": tag,
            "ps": "1",
        },
        token,
    )
    return int(data.get("total", 0))


def quality_gate_status(token: str, project_key: str, pr_number: str) -> str:
    data = api_get(
        "/api/qualitygates/project_status",
        {"projectKey": project_key, "pullRequest": pr_number},
        token,
    )
    return data.get("projectStatus", {}).get("status", "UNKNOWN")


def new_lines(token: str, project_key: str, pr_number: str) -> int:
    data = api_get(
        "/api/measures/component",
        {
            "component": project_key,
            "metricKeys": "new_lines",
            "pullRequest": pr_number,
        },
        token,
    )

    measures = data.get("component", {}).get("measures", [])
    if not measures:
        return 0

    return int(float(measures[0].get("value", "0")))


def changed_lines(base_ref: str) -> int:
    if not base_ref:
        return 0

    subprocess.run(["git", "fetch", "origin", base_ref, "--depth=1"], check=False)
    proc = subprocess.run(
        ["git", "diff", "--numstat", f"origin/{base_ref}...HEAD"],
        check=False,
        capture_output=True,
        text=True,
    )

    total = 0
    for line in proc.stdout.splitlines():
        parts = line.split("\t")
        if len(parts) < 2:
            continue

        added, deleted = parts[0], parts[1]
        if added.isdigit():
            total += int(added)
        if deleted.isdigit():
            total += int(deleted)

    return total


def trx_stats(path: str) -> tuple[int, int, int]:
    if not os.path.exists(path):
        return 0, 0, 0

    root = ET.parse(path).getroot()
    counters = root.find("trx:ResultSummary/trx:Counters", TRX_NS)
    if counters is None:
        return 0, 0, 0

    total = int(counters.attrib.get("total", "0"))
    passed = int(counters.attrib.get("passed", "0"))
    failed = int(counters.attrib.get("failed", "0"))
    return total, passed, failed


def should_zero_score(base_ref: str) -> tuple[bool, list[str]]:
    if not base_ref:
        return False, []

    subprocess.run(["git", "fetch", "origin", base_ref, "--depth=1"], check=False)
    proc = subprocess.run(
        ["git", "diff", "--name-only", f"origin/{base_ref}...HEAD"],
        check=False,
        capture_output=True,
        text=True,
    )

    changed_files = [line.strip() for line in proc.stdout.splitlines() if line.strip()]
    blocked = [
        f for f in changed_files
        if f.startswith("DominoPontaDeQuina.Tests/") or f.startswith(".github/workflows/")
    ]
    return len(blocked) > 0, blocked


def display_score(raw_score: float) -> float:
    return raw_score / 10.0


def main() -> None:
    token = os.environ["SONAR_TOKEN"]
    project_key = os.environ["SONAR_PROJECT_KEY"]
    pr_number = os.environ.get("PR_NUMBER", "")
    base_ref = os.environ.get("BASE_REF", "")

    zero_score, blocked_files = should_zero_score(base_ref)

    summary_path = os.environ["GITHUB_STEP_SUMMARY"]
    with open(summary_path, "a", encoding="utf-8") as summary:
        summary.write("## Critérios de Avaliação\n\n")

        if zero_score:
            summary.write("### Regra de Zeramento Aplicada\n")
            summary.write("Foram detectadas alterações em projeto de testes e/ou workflows. A pontuação total foi zerada.\n")
            summary.write("Arquivos detectados:\n")
            for file in blocked_files:
                summary.write(f"- `{file}`\n")
            return

        qg_status = quality_gate_status(token, project_key, pr_number)
        convention_smells = issues_total(token, project_key, pr_number, "convention")
        documentation_smells = issues_total(token, project_key, pr_number, "documentation")
        total_new_lines = new_lines(token, project_key, pr_number)
        total_changed_lines = changed_lines(base_ref)
        total_scoped_lines = total_new_lines + total_changed_lines

        basic_total, basic_passed, basic_failed = trx_stats("TestResults/basic-tests.trx")
        gap_total, gap_passed, gap_failed = trx_stats("TestResults/gap-tests.trx")
        exception_total, exception_passed, exception_failed = trx_stats("TestResults/exception-tests.trx")

        gap_pass_rate = (gap_passed / gap_total) if gap_total > 0 else 0.0
        exception_pass_rate = (exception_passed / exception_total) if exception_total > 0 else 0.0

        score_pipeline = 50.0 * gap_pass_rate
        if total_scoped_lines <= 0:
            score_documentation = 0.0
            score_convention = 0.0
        else:
            score_documentation = max(0.0, 10.0 * (1.0 - (documentation_smells / total_scoped_lines)))
            score_convention = max(0.0, 10.0 * (1.0 - (convention_smells / total_scoped_lines)))
        score_exception = 10.0 * exception_pass_rate
        score_services = 0.0

        zero_score, blocked_files = should_zero_score(base_ref)
        if zero_score:
            score_pipeline = 0.0
            score_documentation = 0.0
            score_exception = 0.0
            score_services = 0.0
            score_convention = 0.0

        summary.write("| Critério | Peso | Pontuação alcançada | Evidência automática |\n")
        summary.write("|---|---:|---:|---|\n")
        summary.write(
            f"| Pipeline de testes | 50% | **{display_score(score_pipeline):.2f}** | Taxa de aprovação dos testes GAP: **{gap_pass_rate * 100:.2f}%** ({gap_passed}/{gap_total}) |\n"
        )
        summary.write(
            f"| Documentação do código | 10% | **{display_score(score_documentation):.2f}** | Sonar `new_lines` = **{total_new_lines}**, linhas alteradas = **{total_changed_lines}**, issues `documentation` = **{documentation_smells}** |\n"
        )
        summary.write(
            f"| Implementação de exceções customizadas | 10% | **{display_score(score_exception):.2f}** | Taxa de aprovação dos testes `Excecao`: **{exception_pass_rate * 100:.2f}%** ({exception_passed}/{exception_total}) |\n"
        )
        summary.write(
            f"| Aderência às convenções do C# | 10% | **{display_score(score_convention):.2f}** | Sonar `new_lines` = **{total_new_lines}**, linhas alteradas = **{total_changed_lines}**, issues `convention` = **{convention_smells}** |\n"
        )
        summary.write(
            f"| Somatório dos pontos | 80% | **{display_score(score_pipeline + score_documentation + score_exception + score_convention):.2f}** | Pontuação parcial alcançada |\n"
        )
        summary.write("\n")
        summary.write(
            "> Observação: `Criação de serviços e validators organizando a lógica do software` `20%` `0.00` `Avaliação manual (não inferida automaticamente)`.\n"
        )
        summary.write("\n")
        summary.write("### SonarCloud\n")
        summary.write(f"- Quality Gate: **{qg_status}**\n")
        summary.write(f"- Projeto: https://sonarcloud.io/project/overview?id={project_key}\n")


if __name__ == "__main__":
    main()
