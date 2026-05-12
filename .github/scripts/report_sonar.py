import json
import os
import urllib.parse
import urllib.request

SONAR_HOST = "https://sonarcloud.io"


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


def main() -> None:
    token = os.environ["SONAR_TOKEN"]
    project_key = os.environ["SONAR_PROJECT_KEY"]
    pr_number = os.environ.get("PR_NUMBER", "")

    qg_status = quality_gate_status(token, project_key, pr_number)
    convention_smells = issues_total(token, project_key, pr_number, "convention")
    documentation_smells = issues_total(token, project_key, pr_number, "documentation")

    basic_failed = int(os.environ.get("BASIC_FAILED", "0"))
    gap_failed = int(os.environ.get("GAP_FAILED", "0"))
    exception_failed = int(os.environ.get("EXCEPTION_FAILED", "0"))

    summary_path = os.environ["GITHUB_STEP_SUMMARY"]
    with open(summary_path, "a", encoding="utf-8") as summary:
        summary.write("## Critérios de Avaliação\n\n")
        summary.write("| Critério | Peso | Evidência automática |\n")
        summary.write("|---|---:|---|\n")
        summary.write(
            f"| Pipeline de testes | 50% | {'Atendido' if (basic_failed + gap_failed + exception_failed) == 0 else 'Com falhas'} |\n"
        )
        summary.write(
            f"| Documentação do código | 10% | Sonar (tag `documentation`) = **{documentation_smells}** issues no código novo |\n"
        )
        summary.write("| Implementação de exceções customizadas | 10% | Verificado por testes da categoria `Excecao` |\n")
        summary.write("| Criação de serviços e validators organizando a lógica do software | 20% | Avaliação de implementação (revisão de código) |\n")
        summary.write(
            f"| Aderência às convenções do C# | 10% | Sonar (tag `convention`) = **{convention_smells}** issues no código novo |\n"
        )
        summary.write("\n")
        summary.write("### SonarCloud\n")
        summary.write(f"- Quality Gate: **{qg_status}**\n")
        summary.write(f"- Projeto: https://sonarcloud.io/project/overview?id={project_key}\n")


if __name__ == "__main__":
    main()
