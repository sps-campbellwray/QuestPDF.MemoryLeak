using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MemoryLeak.Controllers
{
    [ApiController]
    [Route("pdf")]
    public class PdfController : ControllerBase
    {

        public PdfController()
        {
        }

        [HttpGet]
        public async Task GetImagesAsync()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var images = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));
                });
            }).GenerateImages();
        }

        [HttpGet("parallel")]
        public async Task GetImagesParallelAsync(int count = 10)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Parallel.For(0, count, i =>
            {
                var images = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(20));
                    });
                }).GenerateImages();
            });
        }

        [HttpGet("collect")]
        public async Task CollectGarbage(int iterations = 1000, int wait = 0)
        {
            for (int i = 0; i < iterations; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(wait);
            }
        }

        [HttpGet("no-leak")]
        public async Task GetPdfAsync()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));
                });
            }).GeneratePdf();
        }
    }
}
