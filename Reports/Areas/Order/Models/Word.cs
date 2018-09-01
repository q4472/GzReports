using System;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;

namespace TestWord
{
    public class GeneratedClass
    {
        // Creates a WordprocessingDocument.
        public MemoryStream CreatePackage(DataTable ТабличнаяЧастьСпецификации)
        {
            MemoryStream ms = new MemoryStream();
            using (WordprocessingDocument package = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document))
            {
                CreateParts(package, ТабличнаяЧастьСпецификации);
            }
            return ms;
        }

        // Adds child parts and generates content of the specified part.
        private void CreateParts(WordprocessingDocument document, DataTable ТабличнаяЧастьСпецификации)
        {
            MainDocumentPart mainDocumentPart1 = document.AddMainDocumentPart();
            GenerateMainDocumentPart1Content(mainDocumentPart1, ТабличнаяЧастьСпецификации);

            StyleDefinitionsPart styleDefinitionsPart1 = mainDocumentPart1.AddNewPart<StyleDefinitionsPart>("rId1");
            GenerateStyleDefinitionsPart1Content(styleDefinitionsPart1);
        }

        // Generates content of mainDocumentPart1.
        private void GenerateMainDocumentPart1Content(MainDocumentPart mainDocumentPart1, DataTable ТабличнаяЧастьСпецификации)
        {
            Document document = new Document();

            Body body = document.AppendChild(new Body());
            {
                body.AppendChild(GenerateParagraph("СПЕЦИФИКАЦИЯ", "22", "Center"));
                body.AppendChild(new Paragraph());

                Table table1 = body.AppendChild(new Table());
                {
                    TableProperties tableProperties = new TableProperties()
                    {
                        TableBorders = new TableBorders()
                        {
                            TopBorder = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            LeftBorder = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            BottomBorder = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            RightBorder = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            InsideHorizontalBorder = new InsideHorizontalBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            InsideVerticalBorder = new InsideVerticalBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 }
                        },
                        TableCellMarginDefault = new TableCellMarginDefault()
                        {
                            TopMargin = new TopMargin() { Width = "80", Type = TableWidthUnitValues.Dxa },
                            TableCellLeftMargin = new TableCellLeftMargin() { Width = 80, Type = TableWidthValues.Dxa },
                            BottomMargin = new BottomMargin() { Width = "80", Type = TableWidthUnitValues.Dxa },
                            TableCellRightMargin = new TableCellRightMargin() { Width = 80, Type = TableWidthValues.Dxa },
                        }
                    };
                    table1.Append(tableProperties);

                    table1.AppendChild(new TableGrid());

                    var tr = table1.AppendChild(new TableRow());
                    {
                        tr.AppendChild(GenerateTableCell("№", null, "Center"));
                        tr.AppendChild(GenerateTableCell("Наименование Товара", null, "Center"));
                        tr.AppendChild(GenerateTableCell("Единицы измерения", null, "Center"));
                        tr.AppendChild(GenerateTableCell("Кол-во в единицах измерения", null, "Center"));
                        tr.AppendChild(GenerateTableCell("Зарегистрированная предельная отпускная цена производителя, руб.", null, "Center"));
                        tr.AppendChild(GenerateTableCell("Фактическая отпускная цена производителя без НДС, руб.", null, "Center"));
                        tr.AppendChild(GenerateTableCell("Торговая надбавка, %", null, "Center"));
                        tr.AppendChild(GenerateTableCell("Цена за единицу измерения с учетом всех надбавок, руб.", null, "Center"));
                        tr.AppendChild(GenerateTableCell("Стоимость с учетом всех надбавок, руб.", null, "Center"));
                    }

                    tr = table1.AppendChild(new TableRow());
                    {
                        for (int i = 1; i <= 9; i++)
                        {
                            tr.AppendChild(GenerateTableCell(String.Format("{0}", i), null, "Center"));
                        }
                    }

                    Decimal total = 0;
                    for (int i = 0; i < ТабличнаяЧастьСпецификации.Rows.Count; i++)
                    {
                        DataRow dr = ТабличнаяЧастьСпецификации.Rows[i];
                        Decimal profit = -1;
                        {
                            if (dr["цена_закуп"] != DBNull.Value && dr["цена_по_спецификации"] != DBNull.Value)
                            {
                                Decimal sin = (Decimal)dr["цена_закуп"];
                                Decimal sout = (Decimal)dr["цена_по_спецификации"];
                                if (sin > 0 && sout > sin)
                                {
                                    profit = ((sout - sin) / sin) * 100;
                                }
                            }
                        }
                        Decimal sum = -1;
                        {
                            if (dr["количество"] != DBNull.Value && dr["цена_по_спецификации"] != DBNull.Value)
                            {
                                sum = (Decimal)dr["количество"] * (Decimal)dr["цена_по_спецификации"];
                                total += sum;
                            }
                        }
                        tr = table1.AppendChild(new TableRow());
                        {
                            tr.AppendChild(GenerateTableCell(String.Format("{0}.", i + 1), null, "Center", "Center"));
                            tr.AppendChild(GenerateTableCell(dr["наименование"] as String, null, "Center", "Center"));
                            tr.AppendChild(GenerateTableCell(dr["ед_изм"] as String, null, "Center", "Center"));
                            tr.AppendChild(GenerateTableCell((dr["количество"] != DBNull.Value) ? ((Decimal)dr["количество"]).ToString("n0") : "-", null, "Center", "Center"));
                            tr.AppendChild(GenerateTableCell((dr["рег_цена"] != DBNull.Value) ? ((Decimal)dr["рег_цена"]).ToString("n2") : "-", null, "Center", "Center"));
                            tr.AppendChild(GenerateTableCell((dr["цена_закуп"] != DBNull.Value) ? ((Decimal)dr["цена_закуп"]).ToString("n2") : "-", null, "Center", "Center"));
                            tr.AppendChild(GenerateTableCell((profit >= 0) ? profit.ToString("n2") : "-", null, "Center", "Center"));
                            tr.AppendChild(GenerateTableCell((dr["цена_по_спецификации"] != DBNull.Value) ? ((Decimal)dr["цена_по_спецификации"]).ToString("n2") : "-", null, "Center", "Center"));
                            tr.AppendChild(GenerateTableCell((sum >= 0) ? sum.ToString("n2") : "-", null, "Center", "Center"));
                        }
                    }

                    tr = table1.AppendChild(new TableRow());
                    {
                        tr.AppendChild(GenerateTableCell("Итого:", null, "Right", "Center", 8));
                        tr.AppendChild(GenerateTableCell(total.ToString("n2"), null, "Center", "Center"));
                    }
                }
                Paragraph paragraph4 = body.AppendChild(new Paragraph());
                {
                    paragraph4.AppendChild(
                        new ParagraphProperties(
                            new SectionProperties(
                                new PageSize() { Width = 16838, Height = 11906, Orient = PageOrientationValues.Landscape },
                                new PageMargin() { Top = 1440, Right = 720, Bottom = 720, Left = 720, Header = 360, Footer = 360, Gutter = 0 }
                            )
                        )
                    );
                }
                Paragraph paragraph5 = body.AppendChild(GenerateParagraph("ТЕХНИЧЕСКИЕ ХАРАКТЕРИСТИКИ", "22", "Center"));

                Paragraph paragraph6 = body.AppendChild(new Paragraph());

                Table table2 = new Table();
                {
                    table2.AppendChild(new TableProperties()
                    {
                        TableBorders = new TableBorders()
                        {
                            TopBorder = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            LeftBorder = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            BottomBorder = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            RightBorder = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            InsideHorizontalBorder = new InsideHorizontalBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            InsideVerticalBorder = new InsideVerticalBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 }
                        },
                        TableCellMarginDefault = new TableCellMarginDefault()
                        {
                            TopMargin = new TopMargin() { Width = "100", Type = TableWidthUnitValues.Dxa },
                            TableCellLeftMargin = new TableCellLeftMargin() { Width = 60, Type = TableWidthValues.Dxa },
                            BottomMargin = new BottomMargin() { Width = "100", Type = TableWidthUnitValues.Dxa },
                            TableCellRightMargin = new TableCellRightMargin() { Width = 60, Type = TableWidthValues.Dxa },
                        }
                    });

                    table2.AppendChild(new TableGrid());

                    for (int i = 0; i < ТабличнаяЧастьСпецификации.Rows.Count; i++)
                    {
                        DataRow dr = ТабличнаяЧастьСпецификации.Rows[i];

                        TableRow tableRow7 = table2.AppendChild(new TableRow());
                        {
                            tableRow7.AppendChild(GenerateTableCell("№"+(i+1).ToString(), null, "Center", "Center"));
                            tableRow7.AppendChild(GenerateTableCell("Параметр", null, "Center", "Center", 2));
                            tableRow7.AppendChild(GenerateTableCell("Требуемое значение", null, "Center", "Center", 4));
                        }
                        TableRow tableRow8 = table2.AppendChild(new TableRow());
                        {
                            tableRow8.AppendChild(GenerateTableCell("1.", null, "Center", "Center"));
                            tableRow8.AppendChild(GenerateTableCell("Международное непатентованное наименование", null, null, null, 2));
                            tableRow8.AppendChild(GenerateTableCell(dr["международное_непатентованное_наименование"] as String, null, null, null, 4));
                        }
                        TableRow tableRow9 = table2.AppendChild(new TableRow());
                        {
                            tableRow9.AppendChild(GenerateTableCell("2.", null, "Center", "Center"));
                            tableRow9.AppendChild(GenerateTableCell("Торговое наименование", null, null, null, 2));
                            tableRow9.AppendChild(GenerateTableCell(dr["наименование"] as String, null, null, null, 4));
                        }
                        TableRow tableRow10 = table2.AppendChild(new TableRow());
                        {
                            tableRow10.AppendChild(GenerateTableCell("3.", null, "Center", "Center"));
                            tableRow10.AppendChild(GenerateTableCell("Наименование держателя или владельца регистрационного удостоверения лекарственного препарата, наименование производителя лекарственного препарата", null, null, null, 2));
                            tableRow10.AppendChild(GenerateTableCell((dr["производитель"] as String) + ", " + (dr["производитель"] as String), null, null, null, 4));
                        }
                        TableRow tableRow11 = table2.AppendChild(new TableRow());
                        {
                            tableRow11.AppendChild(GenerateTableCell("4.", null, "Center", "Center"));
                            tableRow11.AppendChild(GenerateTableCell("Номер регистрационного удостоверения лекарственного препарата", null, null, null, 2));
                            tableRow11.AppendChild(GenerateTableCell(dr["номер_ру"] as String, null, null, null, 4));
                        }
                        TableRow tableRow12 = table2.AppendChild(new TableRow());
                        {
                            tableRow12.AppendChild(GenerateTableCell("5.", null, "Center", "Center"));
                            tableRow12.AppendChild(GenerateTableCell("Код в соответствии с Общероссийским классификатором продукции по видам экономической деятельности", null, null, null, 2));
                            tableRow12.AppendChild(GenerateTableCell("", null, null, null, 4));
                        }
                        TableRow tableRow13 = table2.AppendChild(new TableRow());
                        {
                            tableRow13.AppendChild(GenerateTableCell("6.", null, "Center", "Center"));
                            tableRow13.AppendChild(GenerateTableCell("Единица измерения Товара", null, null, null, 2));
                            tableRow13.AppendChild(GenerateTableCell(dr["ед_изм"] as String, null, null, null, 4));
                        }
                        TableRow tableRow14 = table2.AppendChild(new TableRow());
                        {
                            tableRow14.AppendChild(GenerateTableCell("7.", null, "Center", "Center"));
                            tableRow14.AppendChild(GenerateTableCell("Количество Товара в единицах измерения", null, null, null, 2));
                            tableRow14.AppendChild(GenerateTableCell((dr["количество"] != DBNull.Value) ? ((Decimal)dr["количество"]).ToString("n0") : "-", null, null, null, 4));
                        }
                    }

                    TableRow tableRow111 = table2.AppendChild(new TableRow());
                    {
                        tableRow111.AppendChild(GenerateTableCell("8.", null, "Center", "Center"));
                        tableRow111.AppendChild(GenerateTableCell("Информация о Товаре:", null, null, null, 6));
                    }

                    // пункт 8.1.

                    TableRow tableRow112 = table2.AppendChild(new TableRow());
                    {
                        tableRow112.AppendChild(GenerateTableCell("8.1.", null, "Center", "Center"));
                        tableRow112.AppendChild(GenerateTableCell("Товар, произведенный на территории государств - членов Евразийского экономического союза:", null, null, null, 6));
                    }
                    TableRow tableRow113 = table2.AppendChild(new TableRow());
                    {
                        tableRow113.AppendChild(GenerateTableCell("Торговое наименование лекарственного препарата", null, "Center", null, 2));
                        tableRow113.AppendChild(GenerateTableCell("Лекарственная форма, дозировка лекарственного препарата, количество лекарственных форм во вторичной (потребительской) упаковке", null, "Center", null, 2));
                        tableRow113.AppendChild(GenerateTableCell("Наименование страны происхождения Товара (с указанием данных документа, подтверждающего страну происхождения товара – при наличии)", null, "Center"));
                        tableRow113.AppendChild(GenerateTableCell("Единица измерения", null, "Center"));
                        tableRow113.AppendChild(GenerateTableCell("Количество в единицах измерения", null, "Center"));
                    }
                    Decimal total = 0;
                    for (int i = 0; i < ТабличнаяЧастьСпецификации.Rows.Count; i++)
                    {
                        DataRow dr = ТабличнаяЧастьСпецификации.Rows[i];
                        if (dr["страна"] != DBNull.Value
                            && !String.IsNullOrWhiteSpace((String)dr["страна"])
                            && ((String)dr["страна"] == "Россия"
                            || (String)dr["страна"] == "Беларуссия"
                            || (String)dr["страна"] == "Республика Беларусь"
                            || (String)dr["страна"] == "Армения"
                            || (String)dr["страна"] == "Казахстан"
                            || (String)dr["страна"] == "Таджикистан"))
                        {
                            if (dr["количество"] != DBNull.Value && dr["цена_по_спецификации"] != DBNull.Value)
                            {
                                total += (Decimal)dr["количество"] * (Decimal)dr["цена_по_спецификации"];
                            }

                            TableRow tableRow114 = new TableRow();
                            {
                                TableCell tableCell362 = new TableCell();
                                {
                                    TableCellProperties tableCellProperties362 = new TableCellProperties();
                                    GridSpan gridSpan214 = new GridSpan() { Val = 2 };

                                    TableCellVerticalAlignment tableCellVerticalAlignment112 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

                                    tableCellProperties362.Append(gridSpan214);
                                    tableCellProperties362.Append(tableCellVerticalAlignment112);

                                    Paragraph paragraph408 = new Paragraph();

                                    Run run410 = new Run();

                                    Text text398 = new Text();
                                    text398.Text = dr["наименование"] as String;

                                    run410.Append(text398);

                                    paragraph408.Append(run410);

                                    tableCell362.Append(tableCellProperties362);
                                    tableCell362.Append(paragraph408);

                                    tableRow114.Append(tableCell362);
                                }
                                TableCell tableCell363 = new TableCell();
                                {
                                    TableCellProperties tableCellProperties363 = new TableCellProperties();
                                    GridSpan gridSpan215 = new GridSpan() { Val = 2 };

                                    TableCellVerticalAlignment tableCellVerticalAlignment113 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

                                    tableCellProperties363.Append(gridSpan215);
                                    tableCellProperties363.Append(tableCellVerticalAlignment113);

                                    Paragraph paragraph409 = new Paragraph();

                                    Run run411 = new Run();

                                    Text text399 = new Text();
                                    text399.Text = dr["лекарственная_форма_и_дозировка"] as String;

                                    run411.Append(text399);

                                    paragraph409.Append(run411);

                                    tableCell363.Append(tableCellProperties363);
                                    tableCell363.Append(paragraph409);

                                    tableRow114.Append(tableCell363);
                                }
                                TableCell tableCell364 = new TableCell();
                                {
                                    TableCellProperties tableCellProperties364 = new TableCellProperties();

                                    TableCellVerticalAlignment tableCellVerticalAlignment114 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

                                    tableCellProperties364.Append(tableCellVerticalAlignment114);

                                    Paragraph paragraph410 = new Paragraph();

                                    ParagraphProperties paragraphProperties410 = new ParagraphProperties();

                                    paragraphProperties410.Append(new Justification() { Val = JustificationValues.Center });

                                    Run run412 = new Run();

                                    Text text400 = new Text();
                                    text400.Text = dr["страна"] as String;

                                    run412.Append(text400);

                                    paragraph410.Append(paragraphProperties410);
                                    paragraph410.Append(run412);

                                    tableCell364.Append(tableCellProperties364);
                                    tableCell364.Append(paragraph410);

                                    tableRow114.Append(tableCell364);
                                }
                                TableCell tableCell365 = new TableCell();
                                {
                                    TableCellProperties tableCellProperties365 = new TableCellProperties();

                                    TableCellVerticalAlignment tableCellVerticalAlignment115 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

                                    tableCellProperties365.Append(tableCellVerticalAlignment115);

                                    Paragraph paragraph411 = new Paragraph();

                                    ParagraphProperties paragraphProperties411 = new ParagraphProperties();

                                    paragraphProperties411.Append(new Justification() { Val = JustificationValues.Center });

                                    Run run413 = new Run();

                                    Text text401 = new Text();
                                    text401.Text = dr["ед_изм"] as String;

                                    run413.Append(text401);

                                    paragraph411.Append(paragraphProperties411);
                                    paragraph411.Append(run413);

                                    tableCell365.Append(tableCellProperties365);
                                    tableCell365.Append(paragraph411);

                                    tableRow114.Append(tableCell365);
                                }
                                TableCell tableCell366 = new TableCell();
                                {
                                    TableCellProperties tableCellProperties366 = new TableCellProperties();

                                    TableCellVerticalAlignment tableCellVerticalAlignment116 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

                                    tableCellProperties366.Append(tableCellVerticalAlignment116);

                                    Paragraph paragraph412 = new Paragraph();

                                    ParagraphProperties paragraphProperties412 = new ParagraphProperties();

                                    paragraphProperties412.Append(new Justification() { Val = JustificationValues.Center });

                                    Run run414 = new Run();

                                    Text text402 = new Text();
                                    if (dr["количество"] != DBNull.Value)
                                    {
                                        text402.Text = ((Decimal)dr["количество"]).ToString("n3");
                                    }
                                    else { text402.Text = "-"; }

                                    run414.Append(text402);

                                    paragraph412.Append(paragraphProperties412);
                                    paragraph412.Append(run414);

                                    tableCell366.Append(tableCellProperties366);
                                    tableCell366.Append(paragraph412);

                                    tableRow114.Append(tableCell366);
                                }
                                table2.Append(tableRow114);
                            }
                        }
                    }
                    TableRow tableRow117 = new TableRow();
                    {
                        TableCell tableCell373 = new TableCell();

                        TableCellProperties tableCellProperties373 = new TableCellProperties();
                        GridSpan gridSpan219 = new GridSpan() { Val = 6 };

                        tableCellProperties373.Append(gridSpan219);

                        Paragraph paragraph419 = new Paragraph();

                        Run run421 = new Run();

                        Text text409 = new Text();
                        text409.Text = "Итого:";

                        run421.Append(text409);

                        paragraph419.Append(run421);

                        tableCell373.Append(tableCellProperties373);
                        tableCell373.Append(paragraph419);

                        TableCell tableCell374 = new TableCell();

                        Paragraph paragraph420 = new Paragraph();

                        ParagraphProperties paragraphProperties420 = new ParagraphProperties();

                        paragraphProperties420.Append(new Justification() { Val = JustificationValues.Center });

                        Run run422 = new Run();

                        Text text410 = new Text();
                        text410.Text = total.ToString("n2");

                        run422.Append(text410);

                        paragraph420.Append(paragraphProperties420);
                        paragraph420.Append(run422);

                        tableCell374.Append(paragraph420);

                        tableRow117.Append(tableCell373);
                        tableRow117.Append(tableCell374);

                        table2.Append(tableRow117);
                    }

                    // пункт 8.2.

                    TableRow tableRow112_ = new TableRow();
                    {
                        TableCell tableCell355 = new TableCell();

                        TableCellProperties tableCellProperties355 = new TableCellProperties();

                        TableCellVerticalAlignment tableCellVerticalAlignment111 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

                        tableCellProperties355.Append(tableCellVerticalAlignment111);

                        Paragraph paragraph401 = new Paragraph();

                        ParagraphProperties paragraphProperties401 = new ParagraphProperties();

                        paragraphProperties401.Append(new Justification() { Val = JustificationValues.Center });

                        Run run403 = new Run();

                        Text text391 = new Text();
                        text391.Text = "8.2.";

                        run403.Append(text391);

                        paragraph401.Append(paragraphProperties401);
                        paragraph401.Append(run403);

                        tableCell355.Append(tableCellProperties355);
                        tableCell355.Append(paragraph401);

                        TableCell tableCell356 = new TableCell();

                        TableCellProperties tableCellProperties356 = new TableCellProperties();
                        GridSpan gridSpan211 = new GridSpan() { Val = 6 };

                        tableCellProperties356.Append(gridSpan211);

                        Paragraph paragraph402 = new Paragraph();

                        Run run404 = new Run();

                        Text text392 = new Text();
                        text392.Text = "Товар иностранного производства:";

                        run404.Append(text392);

                        paragraph402.Append(run404);

                        tableCell356.Append(tableCellProperties356);
                        tableCell356.Append(paragraph402);

                        tableRow112_.Append(tableCell355);
                        tableRow112_.Append(tableCell356);

                        table2.Append(tableRow112_);
                    }
                    TableRow tableRow113_ = new TableRow();
                    {
                        TableCell tableCell357 = new TableCell();

                        TableCellProperties tableCellProperties357 = new TableCellProperties();
                        GridSpan gridSpan212 = new GridSpan() { Val = 2 };

                        tableCellProperties357.Append(gridSpan212);

                        Paragraph paragraph403 = new Paragraph();

                        ParagraphProperties paragraphProperties403 = new ParagraphProperties();

                        paragraphProperties403.Append(new Justification() { Val = JustificationValues.Center });

                        Run run405 = new Run();

                        Text text393 = new Text();
                        text393.Text = "Торговое наименование лекарственного препарата";

                        run405.Append(text393);

                        paragraph403.Append(paragraphProperties403);
                        paragraph403.Append(run405);

                        tableCell357.Append(tableCellProperties357);
                        tableCell357.Append(paragraph403);

                        TableCell tableCell358 = new TableCell();

                        TableCellProperties tableCellProperties358 = new TableCellProperties();
                        GridSpan gridSpan213 = new GridSpan() { Val = 2 };

                        tableCellProperties358.Append(gridSpan213);

                        Paragraph paragraph404 = new Paragraph();

                        ParagraphProperties paragraphProperties404 = new ParagraphProperties();

                        paragraphProperties404.Append(new Justification() { Val = JustificationValues.Center });

                        Run run406 = new Run();

                        Text text394 = new Text();
                        text394.Text = "Лекарственная форма, дозировка лекарственного препарата, количество лекарственных форм во вторичной (потребительской) упаковке";

                        run406.Append(text394);

                        paragraph404.Append(paragraphProperties404);
                        paragraph404.Append(run406);

                        tableCell358.Append(tableCellProperties358);
                        tableCell358.Append(paragraph404);

                        TableCell tableCell359 = new TableCell();

                        TableCellProperties tableCellProperties359 = new TableCellProperties();

                        Paragraph paragraph405 = new Paragraph();

                        ParagraphProperties paragraphProperties405 = new ParagraphProperties();

                        paragraphProperties405.Append(new Justification() { Val = JustificationValues.Center });

                        Run run407 = new Run();

                        Text text395 = new Text();
                        text395.Text = "Наименование страны происхождения Товара (с указанием данных документа, подтверждающего страну происхождения товара – при наличии)";

                        run407.Append(text395);

                        paragraph405.Append(paragraphProperties405);
                        paragraph405.Append(run407);

                        tableCell359.Append(tableCellProperties359);
                        tableCell359.Append(paragraph405);

                        TableCell tableCell360 = new TableCell();

                        Paragraph paragraph406 = new Paragraph();

                        ParagraphProperties paragraphProperties406 = new ParagraphProperties();

                        paragraphProperties406.Append(new Justification() { Val = JustificationValues.Center });

                        Run run408 = new Run();

                        Text text396 = new Text();
                        text396.Text = "Единица измерения";

                        run408.Append(text396);

                        paragraph406.Append(paragraphProperties406);
                        paragraph406.Append(run408);

                        tableCell360.Append(paragraph406);

                        TableCell tableCell361 = new TableCell();

                        Paragraph paragraph407 = new Paragraph();

                        ParagraphProperties paragraphProperties407 = new ParagraphProperties();

                        paragraphProperties407.Append(new Justification() { Val = JustificationValues.Center });

                        Run run409 = new Run();

                        Text text397 = new Text();
                        text397.Text = "Количество в единицах измерения";

                        run409.Append(text397);

                        paragraph407.Append(paragraphProperties407);
                        paragraph407.Append(run409);

                        tableCell361.Append(paragraph407);

                        tableRow113_.Append(tableCell357);
                        tableRow113_.Append(tableCell358);
                        tableRow113_.Append(tableCell359);
                        tableRow113_.Append(tableCell360);
                        tableRow113_.Append(tableCell361);

                        table2.Append(tableRow113_);
                    }
                    Decimal total_ = 0;
                    for (int i = 0; i < ТабличнаяЧастьСпецификации.Rows.Count; i++)
                    {
                        DataRow dr = ТабличнаяЧастьСпецификации.Rows[i];
                        if (dr["страна"] != DBNull.Value
                            && !String.IsNullOrWhiteSpace((String)dr["страна"])
                            && (String)dr["страна"] != "Россия"
                            && (String)dr["страна"] != "Беларуссия"
                            && (String)dr["страна"] != "Республика Беларусь"
                            && (String)dr["страна"] != "Армения"
                            && (String)dr["страна"] != "Казахстан"
                            && (String)dr["страна"] != "Таджикистан")
                        {
                            if (dr["количество"] != DBNull.Value && dr["цена_по_спецификации"] != DBNull.Value)
                            {
                                total_ += (Decimal)dr["количество"] * (Decimal)dr["цена_по_спецификации"];
                            }

                            TableRow tableRow114 = new TableRow();
                            {
                                TableCell tableCell362 = new TableCell();
                                {
                                    TableCellProperties tableCellProperties362 = new TableCellProperties();
                                    GridSpan gridSpan214 = new GridSpan() { Val = 2 };

                                    TableCellVerticalAlignment tableCellVerticalAlignment112 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

                                    tableCellProperties362.Append(gridSpan214);
                                    tableCellProperties362.Append(tableCellVerticalAlignment112);

                                    Paragraph paragraph408 = new Paragraph();

                                    Run run410 = new Run();

                                    Text text398 = new Text();
                                    text398.Text = dr["наименование"] as String;

                                    run410.Append(text398);

                                    paragraph408.Append(run410);

                                    tableCell362.Append(tableCellProperties362);
                                    tableCell362.Append(paragraph408);

                                    tableRow114.Append(tableCell362);
                                }
                                TableCell tableCell363 = new TableCell();
                                {
                                    TableCellProperties tableCellProperties363 = new TableCellProperties();
                                    GridSpan gridSpan215 = new GridSpan() { Val = 2 };

                                    TableCellVerticalAlignment tableCellVerticalAlignment113 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

                                    tableCellProperties363.Append(gridSpan215);
                                    tableCellProperties363.Append(tableCellVerticalAlignment113);

                                    Paragraph paragraph409 = new Paragraph();

                                    Run run411 = new Run();

                                    Text text399 = new Text();
                                    text399.Text = dr["лекарственная_форма_и_дозировка"] as String;

                                    run411.Append(text399);

                                    paragraph409.Append(run411);

                                    tableCell363.Append(tableCellProperties363);
                                    tableCell363.Append(paragraph409);

                                    tableRow114.Append(tableCell363);
                                }
                                TableCell tableCell364 = new TableCell();
                                {
                                    TableCellProperties tableCellProperties364 = new TableCellProperties();

                                    TableCellVerticalAlignment tableCellVerticalAlignment114 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

                                    tableCellProperties364.Append(tableCellVerticalAlignment114);

                                    Paragraph paragraph410 = new Paragraph();

                                    ParagraphProperties paragraphProperties410 = new ParagraphProperties();

                                    paragraphProperties410.Append(new Justification() { Val = JustificationValues.Center });

                                    Run run412 = new Run();

                                    Text text400 = new Text();
                                    text400.Text = dr["страна"] as String;

                                    run412.Append(text400);

                                    paragraph410.Append(paragraphProperties410);
                                    paragraph410.Append(run412);

                                    tableCell364.Append(tableCellProperties364);
                                    tableCell364.Append(paragraph410);

                                    tableRow114.Append(tableCell364);
                                }
                                TableCell tableCell365 = new TableCell();
                                {
                                    TableCellProperties tableCellProperties365 = new TableCellProperties();

                                    TableCellVerticalAlignment tableCellVerticalAlignment115 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

                                    tableCellProperties365.Append(tableCellVerticalAlignment115);

                                    Paragraph paragraph411 = new Paragraph();

                                    ParagraphProperties paragraphProperties411 = new ParagraphProperties();

                                    paragraphProperties411.Append(new Justification() { Val = JustificationValues.Center });

                                    Run run413 = new Run();

                                    Text text401 = new Text();
                                    text401.Text = dr["ед_изм"] as String;

                                    run413.Append(text401);

                                    paragraph411.Append(paragraphProperties411);
                                    paragraph411.Append(run413);

                                    tableCell365.Append(tableCellProperties365);
                                    tableCell365.Append(paragraph411);

                                    tableRow114.Append(tableCell365);
                                }
                                TableCell tableCell366 = new TableCell();
                                {
                                    TableCellProperties tableCellProperties366 = new TableCellProperties();

                                    TableCellVerticalAlignment tableCellVerticalAlignment116 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

                                    tableCellProperties366.Append(tableCellVerticalAlignment116);

                                    Paragraph paragraph412 = new Paragraph();

                                    ParagraphProperties paragraphProperties412 = new ParagraphProperties();

                                    paragraphProperties412.Append(new Justification() { Val = JustificationValues.Center });

                                    Run run414 = new Run();

                                    Text text402 = new Text();
                                    if (dr["количество"] != DBNull.Value)
                                    {
                                        text402.Text = ((Decimal)dr["количество"]).ToString("n3");
                                    }
                                    else { text402.Text = "-"; }

                                    run414.Append(text402);

                                    paragraph412.Append(paragraphProperties412);
                                    paragraph412.Append(run414);

                                    tableCell366.Append(tableCellProperties366);
                                    tableCell366.Append(paragraph412);

                                    tableRow114.Append(tableCell366);
                                }
                                table2.Append(tableRow114);
                            }
                        }
                    }
                    TableRow tableRow117_ = new TableRow();
                    {
                        TableCell tableCell373 = new TableCell();

                        TableCellProperties tableCellProperties373 = new TableCellProperties();
                        GridSpan gridSpan219 = new GridSpan() { Val = 6 };

                        tableCellProperties373.Append(gridSpan219);

                        Paragraph paragraph419 = new Paragraph();

                        Run run421 = new Run();

                        Text text409 = new Text();
                        text409.Text = "Итого:";

                        run421.Append(text409);

                        paragraph419.Append(run421);

                        tableCell373.Append(tableCellProperties373);
                        tableCell373.Append(paragraph419);

                        TableCell tableCell374 = new TableCell();

                        Paragraph paragraph420 = new Paragraph();

                        ParagraphProperties paragraphProperties420 = new ParagraphProperties();

                        paragraphProperties420.Append(new Justification() { Val = JustificationValues.Center });

                        Run run422 = new Run();

                        Text text410 = new Text();
                        text410.Text = total_.ToString("n2");

                        run422.Append(text410);

                        paragraph420.Append(paragraphProperties420);
                        paragraph420.Append(run422);

                        tableCell374.Append(paragraph420);

                        tableRow117_.Append(tableCell373);
                        tableRow117_.Append(tableCell374);

                        table2.Append(tableRow117_);
                    }
                    TableRow tableRow116 = new TableRow();
                    {
                        TableCell tableCell372 = new TableCell();

                        TableCellProperties tableCellProperties372 = new TableCellProperties();
                        GridSpan gridSpan218 = new GridSpan() { Val = 7 };

                        tableCellProperties372.Append(gridSpan218);

                        Paragraph paragraph418 = new Paragraph();

                        ParagraphProperties paragraphProperties418 = new ParagraphProperties();

                        paragraphProperties418.Append(new Justification() { Val = JustificationValues.Both });

                        Run run420 = new Run();

                        Text text408 = new Text() { Space = SpaceProcessingModeValues.Preserve };
                        text408.Text = "Примечание: в случае применения условий допуска, предусмотренных подпунктом \"г\" пункта 8 приказа Министерства экономического развития Российской Федерации от 25 марта 2014 г. № 155 \"Об условиях допуска товаров, происходящих из иностранных государств, для целей осуществления закупок товаров, работ, услуг для обеспечения государственных и муниципальных нужд\", не допускается замена страны происхождения данных товаров, указанных в заявке, за исключением случая, когда в результате такой замены страной происхождения товаров, будет являться государство - член Евразийского экономического союза. ";

                        run420.Append(text408);

                        paragraph418.Append(paragraphProperties418);
                        paragraph418.Append(run420);

                        tableCell372.Append(tableCellProperties372);
                        tableCell372.Append(paragraph418);

                        tableRow116.Append(tableCell372);

                        table2.Append(tableRow116);
                    }
                    TableRow tableRow118 = new TableRow();
                    {
                        TableRowProperties tableRowProperties1 = new TableRowProperties();
                        TableRowHeight tableRowHeight1 = new TableRowHeight() { Val = (UInt32Value)402U };

                        tableRowProperties1.Append(tableRowHeight1);

                        TableCell tableCell375 = new TableCell();

                        TableCellProperties tableCellProperties375 = new TableCellProperties();

                        TableCellVerticalAlignment tableCellVerticalAlignment122 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

                        tableCellProperties375.Append(tableCellVerticalAlignment122);

                        Paragraph paragraph421 = new Paragraph();

                        ParagraphProperties paragraphProperties421 = new ParagraphProperties();

                        paragraphProperties421.Append(new Justification() { Val = JustificationValues.Center });

                        Run run423 = new Run();

                        Text text411 = new Text();
                        text411.Text = "9.";

                        run423.Append(text411);

                        paragraph421.Append(paragraphProperties421);
                        paragraph421.Append(run423);

                        tableCell375.Append(tableCellProperties375);
                        tableCell375.Append(paragraph421);

                        TableCell tableCell376 = new TableCell();

                        TableCellProperties tableCellProperties376 = new TableCellProperties();
                        GridSpan gridSpan220 = new GridSpan() { Val = 2 };

                        tableCellProperties376.Append(gridSpan220);

                        Paragraph paragraph422 = new Paragraph();

                        Run run424 = new Run();

                        Text text412 = new Text();
                        text412.Text = "Наименование страны происхождения Товара";

                        run424.Append(text412);

                        paragraph422.Append(run424);

                        tableCell376.Append(tableCellProperties376);
                        tableCell376.Append(paragraph422);

                        TableCell tableCell377 = new TableCell();

                        TableCellProperties tableCellProperties377 = new TableCellProperties();
                        GridSpan gridSpan221 = new GridSpan() { Val = 4 };

                        tableCellProperties377.Append(gridSpan221);

                        Paragraph paragraph423 = new Paragraph();

                        tableCell377.Append(tableCellProperties377);
                        tableCell377.Append(paragraph423);

                        tableRow118.Append(tableRowProperties1);
                        tableRow118.Append(tableCell375);
                        tableRow118.Append(tableCell376);
                        tableRow118.Append(tableCell377);

                        table2.Append(tableRow118);
                    }
                    TableRow tableRow119 = new TableRow();
                    {
                        TableCell tableCell378 = new TableCell();

                        TableCellProperties tableCellProperties378 = new TableCellProperties();

                        TableCellVerticalAlignment tableCellVerticalAlignment123 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

                        tableCellProperties378.Append(tableCellVerticalAlignment123);

                        Paragraph paragraph424 = new Paragraph();

                        ParagraphProperties paragraphProperties424 = new ParagraphProperties();

                        paragraphProperties424.Append(new Justification() { Val = JustificationValues.Center });

                        Run run425 = new Run();

                        Text text413 = new Text();
                        text413.Text = "10.";

                        run425.Append(text413);

                        paragraph424.Append(paragraphProperties424);
                        paragraph424.Append(run425);

                        tableCell378.Append(tableCellProperties378);
                        tableCell378.Append(paragraph424);

                        TableCell tableCell379 = new TableCell();

                        TableCellProperties tableCellProperties379 = new TableCellProperties();
                        GridSpan gridSpan222 = new GridSpan() { Val = 2 };

                        tableCellProperties379.Append(gridSpan222);

                        Paragraph paragraph425 = new Paragraph();

                        Run run426 = new Run();

                        Text text414 = new Text();
                        text414.Text = "Остаточный срок годности";

                        run426.Append(text414);

                        paragraph425.Append(run426);

                        tableCell379.Append(tableCellProperties379);
                        tableCell379.Append(paragraph425);

                        TableCell tableCell380 = new TableCell();

                        TableCellProperties tableCellProperties380 = new TableCellProperties();
                        GridSpan gridSpan223 = new GridSpan() { Val = 4 };

                        tableCellProperties380.Append(gridSpan223);

                        Paragraph paragraph426 = new Paragraph();

                        Run run427 = new Run();

                        Text text415 = new Text();
                        text415.Text = "";

                        run427.Append(text415);

                        paragraph426.Append(run427);

                        tableCell380.Append(tableCellProperties380);
                        tableCell380.Append(paragraph426);

                        tableRow119.Append(tableCell378);
                        tableRow119.Append(tableCell379);
                        tableRow119.Append(tableCell380);

                        table2.Append(tableRow119);
                    }

                    body.Append(table2);
                }
                Paragraph paragraph7 = new Paragraph();
                {
                    ParagraphProperties paragraphProperties431 = new ParagraphProperties();

                    paragraphProperties431.Append(new Justification() { Val = JustificationValues.Both });

                    paragraph7.Append(paragraphProperties431);

                    body.Append(paragraph7);
                }
                Paragraph paragraph8 = new Paragraph();
                {
                    ParagraphProperties paragraphProperties437 = new ParagraphProperties();

                    paragraphProperties437.Append(new Justification() { Val = JustificationValues.Center });

                    Run run444 = new Run();

                    RunProperties runProperties443 = new RunProperties();
                    FontSize fontSize880 = new FontSize() { Val = "22" };

                    runProperties443.Append(fontSize880);
                    Text text426 = new Text();
                    text426.Text = "КАЛЕНДАРНЫЙ ПЛАН";

                    run444.Append(runProperties443);
                    run444.Append(text426);

                    paragraph8.Append(paragraphProperties437);
                    paragraph8.Append(run444);

                    body.Append(paragraph8);
                }
                Paragraph paragraph9 = new Paragraph();
                {
                    ParagraphProperties paragraphProperties438 = new ParagraphProperties();

                    paragraphProperties438.Append(new Justification() { Val = JustificationValues.Both });

                    paragraph9.Append(paragraphProperties438);

                    body.Append(paragraph9);
                }
                Table table3 = new Table();
                {
                    table3.AppendChild(new TableProperties()
                    {
                        TableBorders = new TableBorders()
                        {
                            TopBorder = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            LeftBorder = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            BottomBorder = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            RightBorder = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            InsideHorizontalBorder = new InsideHorizontalBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 },
                            InsideVerticalBorder = new InsideVerticalBorder() { Val = BorderValues.Single, Color = "auto", Size = 4, Space = 0 }
                        },
                        TableCellMarginDefault = new TableCellMarginDefault()
                        {
                            TopMargin = new TopMargin() { Width = "100", Type = TableWidthUnitValues.Dxa },
                            TableCellLeftMargin = new TableCellLeftMargin() { Width = 60, Type = TableWidthValues.Dxa },
                            BottomMargin = new BottomMargin() { Width = "100", Type = TableWidthUnitValues.Dxa },
                            TableCellRightMargin = new TableCellRightMargin() { Width = 60, Type = TableWidthValues.Dxa },
                        }
                    });

                    TableGrid tableGrid4 = new TableGrid();
                    {
                        GridColumn gridColumn19 = new GridColumn() { Width = "3022" };
                        GridColumn gridColumn20 = new GridColumn() { Width = "3022" };
                        GridColumn gridColumn21 = new GridColumn() { Width = "3024" };

                        tableGrid4.Append(gridColumn19);
                        tableGrid4.Append(gridColumn20);
                        tableGrid4.Append(gridColumn21);
                    }
                    TableRow tableRow120 = new TableRow();
                    {
                        TableCell tableCell381 = new TableCell();

                        Paragraph paragraph440 = new Paragraph();

                        ParagraphProperties paragraphProperties439 = new ParagraphProperties();

                        paragraphProperties439.Append(new Justification() { Val = JustificationValues.Center });

                        Run run445 = new Run();

                        Text text427 = new Text();
                        text427.Text = "Этап поставки Товара";

                        run445.Append(text427);

                        paragraph440.Append(paragraphProperties439);
                        paragraph440.Append(run445);

                        tableCell381.Append(paragraph440);

                        TableCell tableCell382 = new TableCell();

                        Paragraph paragraph441 = new Paragraph();

                        ParagraphProperties paragraphProperties440 = new ParagraphProperties();

                        paragraphProperties440.Append(new Justification() { Val = JustificationValues.Center });

                        Run run446 = new Run();

                        Text text428 = new Text();
                        text428.Text = "Срок поставки Товара";

                        run446.Append(text428);

                        paragraph441.Append(paragraphProperties440);
                        paragraph441.Append(run446);

                        tableCell382.Append(paragraph441);

                        TableCell tableCell383 = new TableCell();

                        Paragraph paragraph442 = new Paragraph();

                        ParagraphProperties paragraphProperties441 = new ParagraphProperties();

                        paragraphProperties441.Append(new Justification() { Val = JustificationValues.Center });

                        Run run447 = new Run();

                        Text text429 = new Text();
                        text429.Text = "Количество Товара";

                        run447.Append(text429);

                        paragraph442.Append(paragraphProperties441);
                        paragraph442.Append(run447);

                        tableCell383.Append(paragraph442);

                        tableRow120.Append(tableCell381);
                        tableRow120.Append(tableCell382);
                        tableRow120.Append(tableCell383);
                    }
                    TableRow tableRow121 = new TableRow();
                    {
                        TableCell tableCell384 = new TableCell();

                        Paragraph paragraph443 = new Paragraph();

                        Run run448 = new Run();

                        Text text430 = new Text();
                        text430.Text = "1";

                        run448.Append(text430);

                        paragraph443.Append(run448);

                        tableCell384.Append(paragraph443);

                        TableCell tableCell385 = new TableCell();

                        Paragraph paragraph444 = new Paragraph();

                        Run run449 = new Run();

                        Text text431 = new Text();
                        text431.Text = "";

                        run449.Append(text431);

                        paragraph444.Append(run449);

                        tableCell385.Append(paragraph444);

                        TableCell tableCell386 = new TableCell();

                        Paragraph paragraph445 = new Paragraph();

                        Run run450 = new Run();

                        Text text432 = new Text() { Space = SpaceProcessingModeValues.Preserve };
                        text432.Text = "  ";

                        run450.Append(text432);

                        Run run451 = new Run();

                        Text text433 = new Text();
                        text433.Text = "";

                        run451.Append(text433);

                        paragraph445.Append(run450);
                        paragraph445.Append(run451);

                        tableCell386.Append(paragraph445);

                        tableRow121.Append(tableCell384);
                        tableRow121.Append(tableCell385);
                        tableRow121.Append(tableCell386);
                    }
                    table3.Append(tableGrid4);
                    table3.Append(tableRow120);
                    table3.Append(tableRow121);

                    body.Append(table3);
                }
                Paragraph paragraph10 = new Paragraph();
                {
                    ParagraphProperties paragraphProperties445 = new ParagraphProperties();

                    paragraphProperties445.Append(new Justification() { Val = JustificationValues.Both });

                    paragraph10.Append(paragraphProperties445);

                    body.Append(paragraph10);
                }
                SectionProperties sectionProperties4 = new SectionProperties();
                {
                    PageSize pageSize4 = new PageSize() { Width = 11906, Height = 16838, Orient = PageOrientationValues.Portrait };
                    PageMargin pageMargin4 = new PageMargin() { Top = 720, Right = 720, Bottom = 720, Left = 1440, Header = 360, Footer = 360, Gutter = 0 };

                    sectionProperties4.Append(pageSize4);
                    sectionProperties4.Append(pageMargin4);

                    body.Append(sectionProperties4);
                }
            }

            mainDocumentPart1.Document = document;
        }

        // Generates content of styleDefinitionsPart1.
        private void GenerateStyleDefinitionsPart1Content(StyleDefinitionsPart styleDefinitionsPart1)
        {
            styleDefinitionsPart1.Styles = new Styles()
            {
                DocDefaults = new DocDefaults()
                {
                    RunPropertiesDefault = new RunPropertiesDefault()
                    {
                        RunPropertiesBaseStyle = new RunPropertiesBaseStyle()
                        {
                            RunFonts = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                            Languages = new Languages() { Val = "ru-RU" }
                        }
                    },
                    ParagraphPropertiesDefault = new ParagraphPropertiesDefault()
                }
            };
            styleDefinitionsPart1.Styles.AppendChild(new Style()
            {
                Type = StyleValues.Paragraph,
                StyleId = "a",
                Default = true,
                StyleName = new StyleName() { Val = "Normal" },
                PrimaryStyle = new PrimaryStyle(),
                StyleRunProperties = new StyleRunProperties() { FontSize = new FontSize() { Val = "20" } }
            });
        }

        private TableCell GenerateTableCell(
            String text = null,
            String fontSizeVal = null,
            String justificationValues = null,
            String tableVerticalAlignmentValue = null,
            Int32 gridSpan = 0)
        {
            TableCell c = new TableCell();
            if (tableVerticalAlignmentValue != null || gridSpan > 0)
            {
                TableCellProperties p = c.AppendChild(new TableCellProperties());
                switch (tableVerticalAlignmentValue)
                {
                    case "Top": p.AppendChild(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Top }); break;
                    case "Center": p.AppendChild(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }); break;
                    case "Bottom": p.AppendChild(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Bottom }); break;
                    default: break;
                }
                if (gridSpan > 0)
                {
                    p.GridSpan = new GridSpan() { Val = gridSpan };
                    //p.AppendChild(new GridSpan() { Val = gridSpan });
                }
            }
            if (text != null)
            {
                c.AppendChild(GenerateParagraph(text, fontSizeVal, justificationValues));
            }
            return c;
        }

        private Paragraph GenerateParagraph(
            String text = null,
            String fontSizeVal = null,
            String justificationValues = null)
        {
            Paragraph p = new Paragraph();
            switch (justificationValues)
            {
                case "Center": p.AppendChild(new ParagraphProperties(new Justification() { Val = JustificationValues.Center })); break;
                case "Right": p.AppendChild(new ParagraphProperties(new Justification() { Val = JustificationValues.Right })); break;
                case "Both": p.AppendChild(new ParagraphProperties(new Justification() { Val = JustificationValues.Both })); break;
                default: break;
            }
            if (text != null)
            {
                Run r = p.AppendChild(new Run());
                if (fontSizeVal != null)
                {
                    r.AppendChild(new RunProperties(new FontSize() { Val = fontSizeVal }));
                }
                r.AppendChild(new Text(text));
            }
            return p;
        }
    }
}
