from pathlib import Path

from reportlab.lib import colors
from reportlab.lib.enums import TA_CENTER, TA_LEFT
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import ParagraphStyle, getSampleStyleSheet
from reportlab.lib.units import cm
from reportlab.platypus import (
    Image,
    KeepTogether,
    PageBreak,
    Paragraph,
    SimpleDocTemplate,
    Spacer,
    Table,
    TableStyle,
)


ROOT = Path(__file__).resolve().parents[1]
OUTPUT = ROOT / "output" / "pdf" / "Informe_MR222768_desafio03.pdf"
EVIDENCE = ROOT / "docs" / "evidencias"


def header_footer(canvas, doc):
    canvas.saveState()
    canvas.setStrokeColor(colors.HexColor("#D5DEE8"))
    canvas.line(1.7 * cm, 1.35 * cm, A4[0] - 1.7 * cm, 1.35 * cm)
    canvas.setFont("Helvetica", 8)
    canvas.setFillColor(colors.HexColor("#5B6573"))
    canvas.drawString(1.7 * cm, 0.9 * cm, "MR222768_desafío03 - Sistema de pedidos")
    canvas.drawRightString(A4[0] - 1.7 * cm, 0.9 * cm, f"Página {doc.page}")
    canvas.restoreState()


def scaled_image(path: Path, max_width: float, max_height: float) -> Image:
    image = Image(str(path))
    ratio = min(max_width / image.imageWidth, max_height / image.imageHeight)
    image.drawWidth = image.imageWidth * ratio
    image.drawHeight = image.imageHeight * ratio
    return image


def build_pdf():
    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    styles = getSampleStyleSheet()
    styles.add(ParagraphStyle(
        name="CoverTitle",
        parent=styles["Title"],
        fontName="Helvetica-Bold",
        fontSize=25,
        leading=30,
        textColor=colors.HexColor("#173B57"),
        alignment=TA_CENTER,
        spaceAfter=18,
    ))
    styles.add(ParagraphStyle(
        name="CoverSubtitle",
        parent=styles["Normal"],
        fontSize=13,
        leading=18,
        textColor=colors.HexColor("#36566F"),
        alignment=TA_CENTER,
    ))
    styles.add(ParagraphStyle(
        name="Section",
        parent=styles["Heading1"],
        fontName="Helvetica-Bold",
        fontSize=16,
        leading=20,
        textColor=colors.HexColor("#173B57"),
        spaceBefore=8,
        spaceAfter=9,
    ))
    styles.add(ParagraphStyle(
        name="Subsection",
        parent=styles["Heading2"],
        fontName="Helvetica-Bold",
        fontSize=12,
        leading=15,
        textColor=colors.HexColor("#21618C"),
        spaceBefore=7,
        spaceAfter=5,
    ))
    styles.add(ParagraphStyle(
        name="BodyCustom",
        parent=styles["BodyText"],
        fontSize=9.5,
        leading=14,
        textColor=colors.HexColor("#24313C"),
        spaceAfter=6,
    ))
    styles.add(ParagraphStyle(
        name="Small",
        parent=styles["BodyText"],
        fontSize=8,
        leading=11,
        textColor=colors.HexColor("#44515C"),
    ))
    styles.add(ParagraphStyle(
        name="Status",
        parent=styles["BodyText"],
        fontName="Helvetica-Bold",
        fontSize=10,
        leading=14,
        textColor=colors.HexColor("#8A4B08"),
        backColor=colors.HexColor("#FFF4D6"),
        borderColor=colors.HexColor("#E6B84A"),
        borderWidth=0.7,
        borderPadding=8,
        alignment=TA_LEFT,
    ))

    doc = SimpleDocTemplate(
        str(OUTPUT),
        pagesize=A4,
        rightMargin=1.7 * cm,
        leftMargin=1.7 * cm,
        topMargin=1.6 * cm,
        bottomMargin=1.7 * cm,
        title="MR222768_desafío03 - Sistema de pedidos",
        author="Proyecto académico",
        subject="Informe técnico con evidencias reales",
    )

    story = []
    story.extend([
        Spacer(1, 3.2 * cm),
        Paragraph("MR222768_desafío03", styles["CoverTitle"]),
        Paragraph("Sistema de pedidos con ASP.NET Core Web API", styles["CoverSubtitle"]),
        Spacer(1, 0.8 * cm),
        Table([[""]], colWidths=[5.5 * cm], rowHeights=[0.12 * cm], style=TableStyle([
            ("BACKGROUND", (0, 0), (-1, -1), colors.HexColor("#2E86C1")),
        ])),
        Spacer(1, 0.9 * cm),
        Paragraph(
            "Arquitectura de N capas, Entity Framework Core, SQL Server, Docker y pruebas xUnit",
            styles["CoverSubtitle"],
        ),
        Spacer(1, 2.2 * cm),
        Paragraph("Informe técnico con evidencias verificadas el 16 de octubre de 2026", styles["CoverSubtitle"]),
        Spacer(1, 1.2 * cm),
        Paragraph(
            "DOCUMENTO PROVISIONAL: la implementación y las evidencias son reales; falta incorporar la URL definitiva de GitHub y, si corresponde, los datos institucionales de portada.",
            styles["Status"],
        ),
        PageBreak(),
    ])

    story.append(Paragraph("1. Resumen ejecutivo", styles["Section"]))
    story.append(Paragraph(
        "El proyecto implementa una API REST para clientes, productos y pedidos. Incluye persistencia real en SQL Server mediante Entity Framework Core Code First, validaciones con Data Annotations, documentación OpenAPI, ejecución mediante Docker Compose y pruebas unitarias independientes de la base de datos.",
        styles["BodyCustom"],
    ))
    summary_data = [
        ["Componente", "Resultado verificado"],
        ["Docker Desktop", "4.93.0 en ejecución"],
        ["Docker Engine", "29.8.1, Linux/amd64 sobre WSL2"],
        ["SQL Server", "2022, contenedor saludable"],
        ["API", "ASP.NET Core 8 en http://localhost:8080"],
        ["Compilación", "0 errores, 0 advertencias"],
        ["xUnit", "19 aprobadas, 0 fallidas, 0 omitidas"],
        ["Migración", "20261017042631_InitialCreate aplicada"],
    ]
    table = Table(summary_data, colWidths=[5.0 * cm, 11.2 * cm], repeatRows=1)
    table.setStyle(TableStyle([
        ("BACKGROUND", (0, 0), (-1, 0), colors.HexColor("#173B57")),
        ("TEXTCOLOR", (0, 0), (-1, 0), colors.white),
        ("FONTNAME", (0, 0), (-1, 0), "Helvetica-Bold"),
        ("FONTNAME", (0, 1), (0, -1), "Helvetica-Bold"),
        ("FONTSIZE", (0, 0), (-1, -1), 8.5),
        ("GRID", (0, 0), (-1, -1), 0.4, colors.HexColor("#AEBCC8")),
        ("ROWBACKGROUNDS", (0, 1), (-1, -1), [colors.white, colors.HexColor("#F3F7FA")]),
        ("VALIGN", (0, 0), (-1, -1), "MIDDLE"),
        ("LEFTPADDING", (0, 0), (-1, -1), 7),
        ("RIGHTPADDING", (0, 0), (-1, -1), 7),
        ("TOPPADDING", (0, 0), (-1, -1), 6),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 6),
    ]))
    story.extend([Spacer(1, 0.2 * cm), table, Spacer(1, 0.4 * cm)])

    story.append(Paragraph("2. Arquitectura", styles["Section"]))
    story.append(Paragraph(
        "La solución se divide en cuatro capas con dependencias dirigidas hacia el dominio y la aplicación. Los controladores no acceden directamente al DbContext y la lógica de inventario reside en servicios de aplicación.",
        styles["BodyCustom"],
    ))
    arch = [
        ["Capa", "Responsabilidad"],
        ["SistemaPedidos.Domain", "Entidades, validaciones fundamentales y excepciones de dominio."],
        ["SistemaPedidos.Application", "DTOs, contratos de repositorios, servicios y reglas de negocio."],
        ["SistemaPedidos.Infrastructure", "DbContext, configuraciones EF, repositorios, transacciones y migraciones."],
        ["SistemaPedidos.API", "Controladores REST, Swagger, inyección de dependencias y manejo de errores."],
        ["SistemaPedidos.Tests", "Pruebas xUnit con dobles de repositorio; no requieren SQL Server."],
    ]
    arch_table = Table(arch, colWidths=[5.2 * cm, 11.0 * cm], repeatRows=1)
    arch_table.setStyle(TableStyle([
        ("BACKGROUND", (0, 0), (-1, 0), colors.HexColor("#21618C")),
        ("TEXTCOLOR", (0, 0), (-1, 0), colors.white),
        ("FONTNAME", (0, 0), (-1, 0), "Helvetica-Bold"),
        ("FONTNAME", (0, 1), (0, -1), "Helvetica-Bold"),
        ("FONTSIZE", (0, 0), (-1, -1), 8),
        ("GRID", (0, 0), (-1, -1), 0.4, colors.HexColor("#AEBCC8")),
        ("VALIGN", (0, 0), (-1, -1), "TOP"),
        ("ROWBACKGROUNDS", (0, 1), (-1, -1), [colors.white, colors.HexColor("#F3F7FA")]),
        ("PADDING", (0, 0), (-1, -1), 6),
    ]))
    story.append(arch_table)

    story.append(Paragraph("3. Modelo de datos", styles["Section"]))
    model_rows = [
        ["Entidad", "Campos"],
        ["Cliente", "Id, Nombre, Apellido, Email"],
        ["Producto", "Id, Nombre, Descripcion, Precio decimal(18,2), Stock, Version rowversion"],
        ["Pedido", "Id, FechaPedido, Cantidad, ClienteId, ProductoId"],
    ]
    model_table = Table(model_rows, colWidths=[4.2 * cm, 12.0 * cm], repeatRows=1)
    model_table.setStyle(TableStyle([
        ("BACKGROUND", (0, 0), (-1, 0), colors.HexColor("#21618C")),
        ("TEXTCOLOR", (0, 0), (-1, 0), colors.white),
        ("FONTNAME", (0, 0), (-1, 0), "Helvetica-Bold"),
        ("FONTNAME", (0, 1), (0, -1), "Helvetica-Bold"),
        ("FONTSIZE", (0, 0), (-1, -1), 8.3),
        ("GRID", (0, 0), (-1, -1), 0.4, colors.HexColor("#AEBCC8")),
        ("PADDING", (0, 0), (-1, -1), 6),
    ]))
    story.append(model_table)
    story.append(Paragraph(
        "Las relaciones Cliente-Pedido y Producto-Pedido usan claves foráneas con borrado restringido. La migración agrega índices para las claves foráneas, un índice único de email y restricciones CHECK para precio, stock y cantidad.",
        styles["BodyCustom"],
    ))

    story.append(PageBreak())
    story.append(Paragraph("4. API REST", styles["Section"]))
    endpoint_rows = [["Recurso", "Métodos", "Ruta"]]
    for resource, route in [("Clientes", "/api/clientes"), ("Productos", "/api/productos"), ("Pedidos", "/api/pedidos")]:
        endpoint_rows.append([resource, "GET, POST", route])
        endpoint_rows.append([resource, "GET, PUT, DELETE", route + "/{id}"])
    endpoint_table = Table(endpoint_rows, colWidths=[3.4 * cm, 5.1 * cm, 7.7 * cm], repeatRows=1)
    endpoint_table.setStyle(TableStyle([
        ("BACKGROUND", (0, 0), (-1, 0), colors.HexColor("#173B57")),
        ("TEXTCOLOR", (0, 0), (-1, 0), colors.white),
        ("FONTNAME", (0, 0), (-1, 0), "Helvetica-Bold"),
        ("FONTSIZE", (0, 0), (-1, -1), 8.5),
        ("GRID", (0, 0), (-1, -1), 0.4, colors.HexColor("#AEBCC8")),
        ("ROWBACKGROUNDS", (0, 1), (-1, -1), [colors.white, colors.HexColor("#F3F7FA")]),
        ("PADDING", (0, 0), (-1, -1), 6),
    ]))
    story.append(endpoint_table)
    story.append(Paragraph("Códigos HTTP utilizados", styles["Subsection"]))
    story.append(Paragraph(
        "200 para consultas y actualizaciones; 201 para creaciones; 204 para eliminaciones; 400 para validaciones; 404 para registros inexistentes; 409 para conflictos de negocio o concurrencia; y 500 solo para fallos inesperados sin detalles internos.",
        styles["BodyCustom"],
    ))

    story.append(Paragraph("5. Reglas de negocio y concurrencia", styles["Section"]))
    rules = [
        "No se crean pedidos para clientes o productos inexistentes.",
        "La cantidad y el precio deben ser mayores que cero; el stock no puede ser negativo.",
        "Un pedido se rechaza cuando el inventario es insuficiente.",
        "Crear, actualizar o eliminar pedidos ajusta automáticamente el stock.",
        "Cambiar el producto de un pedido restaura el inventario anterior y descuenta el nuevo.",
        "No se eliminan clientes ni productos con pedidos asociados.",
        "Los cambios del pedido y del inventario se guardan en una transacción Serializable.",
        "Producto.Version usa rowversion para detectar concurrencia y evitar sobreventa.",
    ]
    for rule in rules:
        story.append(Paragraph("• " + rule, styles["BodyCustom"]))

    story.append(Paragraph("6. Pruebas automatizadas", styles["Section"]))
    story.append(Paragraph(
        "La ejecución real de xUnit finalizó con 19 pruebas aprobadas, 0 fallidas y 0 omitidas. Las pruebas cubren validación por Data Annotations, creación de entidades, consultas existentes e inexistentes, stock insuficiente, descuento y restauración de inventario, actualización de cantidades, cambio de producto y restricciones de eliminación.",
        styles["BodyCustom"],
    ))
    test_box = Table([
        ["Resultado xUnit", "19 aprobadas"],
        ["Fallidas", "0"],
        ["Omitidas", "0"],
        ["Compilación", "0 errores / 0 advertencias"],
    ], colWidths=[7.2 * cm, 9.0 * cm])
    test_box.setStyle(TableStyle([
        ("BACKGROUND", (0, 0), (0, -1), colors.HexColor("#EAF4EA")),
        ("TEXTCOLOR", (0, 0), (-1, -1), colors.HexColor("#1D4D2A")),
        ("FONTNAME", (0, 0), (0, -1), "Helvetica-Bold"),
        ("GRID", (0, 0), (-1, -1), 0.5, colors.HexColor("#7DB68A")),
        ("PADDING", (0, 0), (-1, -1), 7),
    ]))
    story.append(test_box)

    story.append(PageBreak())
    story.append(Paragraph("7. Pruebas HTTP de integración", styles["Section"]))
    http_rows = [
        ["Caso", "Resultado"],
        ["Swagger/OpenAPI", "200"],
        ["Crear / consultar / actualizar cliente", "201 / 200 / 200"],
        ["Crear / consultar / actualizar producto", "201 / 200 / 200"],
        ["Crear / consultar pedido", "201 / 200"],
        ["Stock después de pedido de 3", "12 a 9"],
        ["Datos inválidos", "400"],
        ["Stock insuficiente", "409"],
        ["Registro inexistente", "404"],
        ["Eliminar cliente/producto asociado", "409 / 409"],
        ["Actualizar pedido a 5 unidades", "200; stock a 7"],
        ["Eliminar pedido", "204; stock restaurado a 12"],
        ["Limpieza de datos de prueba", "204 / 204"],
    ]
    http_table = Table(http_rows, colWidths=[10.6 * cm, 5.6 * cm], repeatRows=1)
    http_table.setStyle(TableStyle([
        ("BACKGROUND", (0, 0), (-1, 0), colors.HexColor("#173B57")),
        ("TEXTCOLOR", (0, 0), (-1, 0), colors.white),
        ("FONTNAME", (0, 0), (-1, 0), "Helvetica-Bold"),
        ("FONTSIZE", (0, 0), (-1, -1), 8.3),
        ("GRID", (0, 0), (-1, -1), 0.4, colors.HexColor("#AEBCC8")),
        ("ROWBACKGROUNDS", (0, 1), (-1, -1), [colors.white, colors.HexColor("#F3F7FA")]),
        ("PADDING", (0, 0), (-1, -1), 5),
    ]))
    story.append(http_table)

    story.append(PageBreak())
    story.append(Paragraph("8. Evidencia visual: Docker", styles["Section"]))
    story.append(Paragraph(
        "Captura real de Docker Desktop. El grupo sistema-pedidos se encuentra activo; la verificación por CLI confirmó ambos contenedores y el estado saludable de SQL Server.",
        styles["BodyCustom"],
    ))
    story.append(scaled_image(EVIDENCE / "09-docker-desktop.png", 16.0 * cm, 20.5 * cm))

    story.append(PageBreak())
    story.append(Paragraph("9. Evidencia visual: Swagger", styles["Section"]))
    story.append(Paragraph(
        "Captura real de la interfaz OpenAPI servida por la API en http://localhost:8080/swagger. Se observan los controladores de Clientes, Pedidos y Productos con sus operaciones REST.",
        styles["BodyCustom"],
    ))
    story.append(scaled_image(EVIDENCE / "10-swagger.png", 16.2 * cm, 20.8 * cm))

    story.append(PageBreak())
    story.append(Paragraph("10. Ejecución y despliegue", styles["Section"]))
    story.append(Paragraph(
        "Docker Compose configura una red interna, un volumen persistente, SQL Server 2022 y la API. La conexión utiliza el nombre del servicio sqlserver y la contraseña se mantiene exclusivamente en el archivo local .env excluido por Git. El healthcheck espera a SQL Server y la API aplica las migraciones con reintentos al iniciar.",
        styles["BodyCustom"],
    ))
    story.append(Paragraph("11. Conclusiones", styles["Section"]))
    story.append(Paragraph(
        "La solución cumple el alcance solicitado y fue verificada de extremo a extremo. La separación por capas mantiene la lógica de negocio fuera de los controladores, EF Core proporciona persistencia y migraciones reproducibles, y la combinación de transacciones serializables con rowversion protege el inventario frente a operaciones concurrentes. Las pruebas unitarias y HTTP confirmaron las reglas principales y los códigos de respuesta.",
        styles["BodyCustom"],
    ))
    story.append(Paragraph("12. Información pendiente", styles["Section"]))
    pending = [
        ["URL del repositorio GitHub", "____________________________________________"],
        ["Datos institucionales de portada", "____________________________________________"],
    ]
    pending_table = Table(pending, colWidths=[6.0 * cm, 10.2 * cm])
    pending_table.setStyle(TableStyle([
        ("BACKGROUND", (0, 0), (0, -1), colors.HexColor("#FFF4D6")),
        ("FONTNAME", (0, 0), (0, -1), "Helvetica-Bold"),
        ("GRID", (0, 0), (-1, -1), 0.5, colors.HexColor("#D6B656")),
        ("PADDING", (0, 0), (-1, -1), 8),
        ("FONTSIZE", (0, 0), (-1, -1), 9),
    ]))
    story.append(pending_table)
    story.append(Spacer(1, 0.5 * cm))
    story.append(Paragraph(
        "Anexos disponibles en docs/evidencias/: salidas completas de restore, build y test; resultados HTTP en JSON; migración consultada en SQL Server; logs de la API; capturas y especificación OpenAPI.",
        styles["Small"],
    ))

    doc.build(story, onFirstPage=header_footer, onLaterPages=header_footer)
    print(OUTPUT)


if __name__ == "__main__":
    build_pdf()
