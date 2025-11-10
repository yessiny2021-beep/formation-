using MvcMovie.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;

namespace MvcMovie.Documents
{
    public class CertificateDocument : IDocument
    {
        private readonly Inscription _inscription;

        public CertificateDocument(Inscription inscription)
        {
            _inscription = inscription;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.PageColor(Colors.White); // ✅ remplace Background obsolète

                page.Header()
                    .Text("🎓 Attestation de Formation")
                    .FontSize(24)
                    .SemiBold()
                    .AlignCenter();

                page.Content().PaddingVertical(40).Column(col =>
                {
                    col.Spacing(20);

                    col.Item().Text($"Ceci certifie que l'employé :")
                        .FontSize(14);

                    col.Item().Text($"{_inscription.Employe!.Nom} {_inscription.Employe!.Prenom}") // ✅ null-forgiving
                        .FontSize(18).Bold().FontColor(Colors.Blue.Medium);

                    col.Item().Text($"a suivi avec succès la formation :")
                        .FontSize(14);

                    col.Item().Text($"{_inscription.Formation!.Titre}") // ✅ null-forgiving
                        .FontSize(18).Bold().FontColor(Colors.Green.Medium);

                    col.Item().Text($"Date : {_inscription.DateInscription:dd/MM/yyyy}")
                        .FontSize(12).Italic();

                    col.Item().Text("Félicitations pour votre réussite !")
                        .FontSize(14).FontColor(Colors.Orange.Darken2).AlignCenter();
                });

                page.Footer()
                    .AlignCenter()
                    .Text($"Certificat généré le {DateTime.Now:dd/MM/yyyy}");
            });
        }
    }
}
