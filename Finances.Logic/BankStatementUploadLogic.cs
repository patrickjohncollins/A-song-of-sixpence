using Finances.Data.Banking;
using Finances.Logic.Qif;
using OFXSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Logic
{

    // TODO : The appropriate post processor needs to be selected based on the bank.
    // I might be able to determine the bank from the BankID field, but I wouldn't know the country, are these unique internationally?

    public class BankStatementUploadLogic
    {
        public BankStatement[] UploadStatements(string fileName, Stream fileStream)
        {
            var ofxDocuments = ProcessStatement(fileName, fileStream);
            var ofxImporter = new OfxImporter();
            var statements = ofxImporter.ImportOfx(ofxDocuments);
            return statements;
        }

        public OFXDocument[] ProcessStatement(string fileName, Stream fileStream)
        {
            if (fileName.EndsWith("qif"))
                return ProcessQifStatement(fileStream);
            else if (fileName.EndsWith("ofx"))
                return ProcessOfxStatement(fileStream);
            else
                throw new ApplicationException("Unrecognized statement format.");
        }

        public OFXDocument[] ProcessQifStatement(Stream fileStream)
        {
            OFXDocument[] ofxDocuments;
            var qifToOfx = new QifToOfx();
            ofxDocuments = new OFXSharp.OFXDocument[] { qifToOfx.Convert(fileStream) };
            var processor = new Ofx.PostProcessing.Boursorama.PostProcessor();
            processor.Process(ofxDocuments);
            return ofxDocuments;
        }

        public OFXDocument[] ProcessOfxStatement(Stream fileStream)
        {
            OFXDocument[] ofxDocuments;
            var parser = new OFXDocumentParser();
            ofxDocuments = parser.Import(fileStream);
            var processor = new Ofx.PostProcessing.SocieteGenerale.PostProcessor();
            processor.Process(ofxDocuments);
            return ofxDocuments;
        }

    }
}
