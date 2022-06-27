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
        private const string BallotChoiceSymbol = "□";

        /// <summary>
        /// Current election.
        /// </summary>
        public Election Election { get; } // TODO: integrate into paper ballot when storing

        /// <summary>
        /// Paper ballot containing options in plain text.
        /// </summary>
        public PaperBallot PaperBallot { get; }

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
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(s => s.FontSize(14).FontFamily("FreeSans"));

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
                    h.Cell().Element(CellStyle);
                    h.Cell().Element(CellStyle);
                    h.Cell().Element(CellStyle).Text(BallotChoiceSymbol);
                    h.Cell().Element(CellStyle).Text(BallotChoiceSymbol);

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1);
                    }
                });

                foreach (PaperBallotOption option in PaperBallot.Options)
                {
                    t.Cell().Element(CellStyle).Text(option.Name);
                    t.Cell().Element(CellStyle).Text(BallotChoiceSymbol);
                    t.Cell().Element(CellStyle).Text(option.ShortCode1);
                    t.Cell().Element(CellStyle).Text(option.ShortCode2);

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1);
                    }
                }
            });
        }

        private void ComposeBallotCode(IContainer container)
        {
            container.PaddingVertical(1, Unit.Centimetre).DefaultTextStyle(s => s.FontSize(10).FontFamily("FreeMono").FontColor("#484848")).Text(PaperBallot.BallotId);
        }

        private byte[] GenerateQrCode()
        {
            BallotQrData ballotQr = new BallotQrData(Election.Id, PaperBallot.BallotId);

            string jsonQrData = JsonConvert.SerializeObject(ballotQr);

            QRCodeGenerator generator = new QRCodeGenerator();

            QRCodeData data = generator.CreateQrCode(jsonQrData, QRCodeGenerator.ECCLevel.Q);

            BitmapByteQRCode qr = new BitmapByteQRCode(data);

            return qr.GetGraphic(20);
        }
    }
}
