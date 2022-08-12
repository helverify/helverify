using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Newtonsoft.Json;
using QRCoder;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Helverify.VotingAuthority.Domain.Template
{
    /// <summary>
    /// According to this tutorial: https://www.questpdf.com/documentation/getting-started.html
    /// </summary>
    public class PaperBallotTemplate : IDocument
    {
        private const string BallotChoiceSymbolPath = "graphics/square.png";
        private const float BallotChoiceSize = 0.4f;
        private static byte[] BallotChoiceSymbol;

        /// <summary>
        /// Current election.
        /// </summary>
        public Election Election { get; }

        /// <summary>
        /// Paper ballot containing options in plain text.
        /// </summary>
        public PaperBallot PaperBallot { get; }

        private byte[] _qrCode;


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
            _qrCode = GenerateQrCode();
        }

        /// <inheritdoc cref="IDocument.GetMetadata"/>
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        /// <inheritdoc cref="IDocument.Compose"/>
        public void Compose(IDocumentContainer container)
        {
            // Page 1: ballot original
            container.Page(CreateBallotPage(false));

            // Page 2: ballot copy
            container.Page(CreateBallotPage(true));
        }

        private Action<PageDescriptor> CreateBallotPage(bool isCopy)
        {
            return page =>
            {
                page.Size(PageSizes.A5.Landscape());
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(s => s.FontSize(14).FontFamily("Karla"));

                page.Header().Element(ComposeHeader);

                page.Content().Element(container1 => ComposeContent(container1, isCopy));
            };
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
                r.ConstantItem(80).Height(80).Image(_qrCode);
            });
        }

        private void ComposeContent(IContainer container, bool isCopy)
        {
            container.Column(c =>
            {
                if (isCopy)
                {
                    c.Item().Element(ComposeCopy);
                }
                c.Item().Element(ComposeChoices);
                c.Item().Element(ComposeBallotCode);
            });
        }

        private void ComposeCopy(IContainer container)
        {
            container.Unconstrained().PaddingHorizontal(5.5f, Unit.Centimetre).Text("COPY").FontSize(100).FontColor("#d95d5d");
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
            BallotQrData ballotQr = new BallotQrData(Election.Id!, PaperBallot.BallotId, Election.ContractAddress);

            string jsonQrData = JsonConvert.SerializeObject(ballotQr);

            QRCodeGenerator generator = new QRCodeGenerator();

            QRCodeData data = generator.CreateQrCode(jsonQrData, QRCodeGenerator.ECCLevel.Q);

            BitmapByteQRCode qr = new BitmapByteQRCode(data);

            return qr.GetGraphic(1);
        }
    }
}
