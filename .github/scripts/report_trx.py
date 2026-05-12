import os
import xml.etree.ElementTree as ET

NS = {"trx": "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}


def parse_trx(path: str):
    tree = ET.parse(path)
    root = tree.getroot()

    counters = root.find("trx:ResultSummary/trx:Counters", NS)
    if counters is None:
        total = passed = failed = skipped = 0
    else:
        total = int(counters.attrib.get("total", "0"))
        passed = int(counters.attrib.get("passed", "0"))
        failed = int(counters.attrib.get("failed", "0"))
        skipped = int(counters.attrib.get("notExecuted", "0"))

    percentage = (passed / total * 100) if total > 0 else 0.0

    results = root.findall("trx:Results/trx:UnitTestResult", NS)
    rows = []
    for result in results:
        name = result.attrib.get("testName", "(sem nome)")
        outcome = result.attrib.get("outcome", "Unknown")
        rows.append((name, outcome))

    rows.sort(key=lambda item: item[0].lower())

    return {
        "total": total,
        "passed": passed,
        "failed": failed,
        "skipped": skipped,
        "percentage": percentage,
        "rows": rows,
    }


def write_section(summary_file, title: str, data):
    summary_file.write(f"### {title}\n")
    summary_file.write(f"- Total: {data['total']}\n")
    summary_file.write(f"- Passou: {data['passed']}\n")
    summary_file.write(f"- Falhou: {data['failed']}\n")
    summary_file.write(f"- Ignorados: {data['skipped']}\n")
    summary_file.write(f"- Percentual de aprovação: **{data['percentage']:.2f}%**\n\n")

    summary_file.write("| Teste | Status |\n")
    summary_file.write("|---|---|\n")
    for name, outcome in data["rows"]:
        safe_name = name.replace("|", "\\|")
        summary_file.write(f"| {safe_name} | {outcome} |\n")
    summary_file.write("\n")


def main():
    basic_path = "TestResults/basic-tests.trx"
    gap_path = "TestResults/gap-tests.trx"
    exception_path = "TestResults/exception-tests.trx"

    basic = parse_trx(basic_path)
    gap = parse_trx(gap_path)
    exception = parse_trx(exception_path)

    gap_contrib = gap["percentage"] * 0.5

    summary_path = os.environ["GITHUB_STEP_SUMMARY"]
    with open(summary_path, "a", encoding="utf-8") as summary:
        summary.write("## Relatório de Execução dos Testes\n\n")
        summary.write("Separação por label de categoria:\n")
        summary.write("- `Categoria=Basico`: testes de referência (regressão)\n")
        summary.write("- `Categoria=Gap`: testes avaliativos dos gaps de implementação\n")
        summary.write("- `Categoria=Excecao`: testes que validam disparo de exceções do projeto \n\n")

        write_section(summary, "Testes Básicos", basic)
        write_section(summary, "Testes de Gap", gap)
        write_section(summary, "Testes de Exceção", exception)

        summary.write("### Critério de Nota (Gaps = 50%)\n")
        summary.write(f"- Percentual dos testes de gap: **{gap['percentage']:.2f}%**\n")
        summary.write(f"- Contribuição na nota final (50%): **{gap_contrib:.2f} pontos percentuais**\n")

    output_path = os.environ["GITHUB_OUTPUT"]
    with open(output_path, "a", encoding="utf-8") as output:
        output.write(f"basic_failed={basic['failed']}\n")
        output.write(f"gap_failed={gap['failed']}\n")
        output.write(f"exception_failed={exception['failed']}\n")
        output.write(f"gap_percentage={gap['percentage']:.2f}\n")


if __name__ == "__main__":
    main()

