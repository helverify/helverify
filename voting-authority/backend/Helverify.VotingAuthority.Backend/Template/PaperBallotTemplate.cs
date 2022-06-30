using System.Drawing;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Newtonsoft.Json;
using QRCoder;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Helverify.VotingAuthority.Backend.Template
{
    /// <summary>
    /// According to this tutorial: https://www.questpdf.com/documentation/getting-started.html
    /// </summary>
    public class PaperBallotTemplate : IDocument
    {
        private const string BallotChoiceSymbolPath = "/app/graphics/square.png";
        private const float BallotChoiceSize = 0.4f;
        private static byte[] BallotChoiceSymbol;

        /// <summary>
        /// Current election.
        /// </summary>
        public Election Election { get; } // TODO: integrate into paper ballot when storing

        /// <summary>
        /// Paper ballot containing options in plain text.
        /// </summary>
        public PaperBallot PaperBallot { get; }


        static PaperBallotTemplate()
        {
            BallotChoiceSymbol = File.ReadAllBytes(BallotChoiceSymbolPath);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="election">Election</param>
        /// <param name="paperBallot">Paper ballot to be printed</param>
        public PaperBallotTemplate(Election election, PaperBallot paperBallot)
        {
            Election = election;
            PaperBallot = paperBallot;
        }

        /// <inheritdoc cref="IDocument.GetMetadata"/>
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        /// <inheritdoc cref="IDocument.Compose"/>
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5.Landscape());
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(s => s.FontSize(14).FontFamily("Karla"));

                page.Header().Element(ComposeHeader);

                page.Content().Element(ComposeContent);

            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(r =>
            {
                r.RelativeItem().Column(c =>
                {
                    c.Item().Text(Election.Name)
                        .Style(TextStyle.Default.FontSize(18).SemiBold().FontColor(Colors.Black));

                    c.Item().Text(Election.Question).SemiBold();
                });
                r.ConstantItem(80).Height(80).Image(GenerateQrCode());
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.Column(c =>
            {
                c.Item().Element(ComposeChoices);
                c.Item().Element(ComposeBallotCode);
            });
        }

        private void ComposeChoices(IContainer container)
        {
            container.Table(t =>
            {
                t.ColumnsDefinition(c =>
                {
                    c.RelativeColumn();
                    c.RelativeColumn();
                    c.RelativeColumn();
                    c.RelativeColumn();
                });

                t.Header(h =>
                {
                    h.Cell().Element(HeaderStyleRegular);
                    h.Cell().Element(HeaderStyleRegular);
                    h.Cell().Element(HeaderStyleBallotChoice).Width(BallotChoiceSize, Unit.Centimetre).Height(BallotChoiceSize, Unit.Centimetre).Image(BallotChoiceSymbol);
                    h.Cell().Element(HeaderStyleBallotChoice).Width(BallotChoiceSize, Unit.Centimetre).Height(BallotChoiceSize, Unit.Centimetre).Image(BallotChoiceSymbol);

                    static IContainer HeaderStyleRegular(IContainer container)
                    {
                        return container.BorderBottom(1);
                    }

                    static IContainer HeaderStyleBallotChoice(IContainer container)
                    {
                        return container.BorderBottom(1).PaddingBottom(0.1f, Unit.Centimetre).PaddingLeft(0.1f, Unit.Centimetre).PaddingRight(0.1f, Unit.Centimetre);
                    }
                });

                foreach (PaperBallotOption option in PaperBallot.Options)
                {
                    t.Cell().Element(CellStyleRegular).Text(option.Name);
                    t.Cell().Element(CellStyleBallotChoice).Width(BallotChoiceSize, Unit.Centimetre).Height(BallotChoiceSize, Unit.Centimetre).Image(BallotChoiceSymbol);
                    t.Cell().Element(CellStyleRegular).Text(option.ShortCode1);
                    t.Cell().Element(CellStyleRegular).Text(option.ShortCode2);

                    static IContainer CellStyleRegular(IContainer container)
                    {
                        return container.BorderBottom(1);
                    }

                    static IContainer CellStyleBallotChoice(IContainer container)
                    {
                        return container.BorderBottom(1).PaddingTop(0.2f, Unit.Centimetre).PaddingBottom(0.2f, Unit.Centimetre);
                    }
                }
            });
        }

        private void ComposeBallotCode(IContainer container)
        {
            container.PaddingVertical(1, Unit.Centimetre).DefaultTextStyle(s => s.FontSize(10).FontFamily("Karla").FontColor("#484848")).Text(PaperBallot.BallotId);
        }

        private byte[] GenerateQrCode()
        {
            BallotQrData ballotQr = new BallotQrData(Election.Id, PaperBallot.BallotId);

            string jsonQrData = JsonConvert.SerializeObject(ballotQr);

            QRCodeGenerator generator = new QRCodeGenerator();

            QRCodeData data = generator.CreateQrCode(jsonQrData, QRCodeGenerator.ECCLevel.Q);

            BitmapByteQRCode qr = new BitmapByteQRCode(data);

            return qr.GetGraphic(1);
        }
    }
}
