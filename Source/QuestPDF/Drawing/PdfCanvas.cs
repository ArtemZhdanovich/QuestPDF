﻿using System;
using System.IO;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Drawing
{
    internal sealed class PdfCanvas : SkiaDocumentCanvasBase
    {
        public PdfCanvas(SkWriteStream stream, DocumentMetadata documentMetadata, DocumentSettings documentSettings) 
            : base(CreatePdf(stream, documentMetadata, documentSettings))
        {
            
        }

        private static SkDocument CreatePdf(SkWriteStream stream, DocumentMetadata documentMetadata, DocumentSettings documentSettings)
        {
            try
            {
                return SkPdfDocument.Create(stream, MapMetadata(documentMetadata, documentSettings));
            }
            catch (TypeInitializationException exception)
            {
                throw new InitializationException("PDF", exception);
            }
        }

        private static SkPdfDocumentMetadata MapMetadata(DocumentMetadata metadata, DocumentSettings documentSettings)
        {
            using var title = new SkText(metadata.Title);
            using var author = new SkText(metadata.Author);
            using var subject = new SkText(metadata.Subject);
            using var keywords = new SkText(metadata.Keywords);
            using var creator = new SkText(metadata.Creator);
            using var producer = new SkText(metadata.Producer);
            
            return new SkPdfDocumentMetadata
            {
                Title = title,
                Author = author,
                Subject = subject,
                Keywords = keywords,
                Creator = creator,
                Producer = producer,
                
                CreationDate = new SkDateTime(metadata.CreationDate),
                ModificationDate = new SkDateTime(metadata.ModifiedDate),
                
                RasterDPI = documentSettings.ImageRasterDpi,
                ImageEncodingQuality = documentSettings.ImageCompressionQuality.ToQualityValue(),
                
                SupportPDFA = documentSettings.PdfA,
                CompressDocument = documentSettings.CompressDocument
            };
        }
    }
}