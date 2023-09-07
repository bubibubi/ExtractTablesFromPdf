using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildTablesFromPdf.Engine.Test
{
    [TestClass]
    public class PdfArrayDataTypeTest
    {
        [TestMethod]
        public void TestGetRawDataBaseTest()
        {
            var inputData = "[(:)19(-)] TJ\n0 -13.65 Td\n(\"$$7.#8-8:;9-) Tj\n0 -14.7 Td\n(+) Tj\n[(8)75(\")75(89<9#8-6)(\")75(89-)] TJ\n144 29.2 Td\n(\"96) Tj\n0 -14.65 Td\n($.!!9#8) Tj\n[(-)31(\"$$7.#8)] TJ\n-65.6 -12.1 Td\n/c 7 Tf\n(=>?@\\)*-ABC-DE'>F) Tj\n17.55 12.75 Td\n(=>?@\\)*-&GH) Tj\n14.95 12.5 Td\n(IJKL\\)*) Tj\n-13.8 30 Td\n(=>?@\\)*-MN') Tj\n46.9 -56.85 Td\n/c 8 Tf\n(O5POQP4O45) Tj\n-144 40 Td\n(/ \"#-#7-) Tj\n144 0.85 Td\n(\"931-O42O-OR1S-4O41-13R3-5OQ) Tj\n0 14.75 Td\n(OR1S4O4113R35OQ) Tj\n-10.45 -15.6 Td\n(T) Tj\n0 -12.5 Td\n(T) Tj\n0 -14.65 Td\n(T) Tj\n0 -12.5 Td\n(T) Tj\n0.1 55.25 Td\n(T) Tj\n-77.05 -14.1 Td\n/c 7 Tf\n(UV'WK\\)*-=>?@J\\)-U\\)GX\\)*-MN'\\)*) Tj\nET\nQ\nBT\n1 0 0 1 240.35 -205.7 Tm\n0 0 0 sc\n/c 8 Tf\n(Y!7<-) Tj\n115.15 1.95 Td\n(ZK) Tj\n17.85 -0.25 Td\n(T) Tj\n9.6 1.1 Td\n(OQPO[P4O44) Tj\n63.7 -0.85 Td\n[(8)75(\\\\)] TJ\n23.85 0 Td\n(U\\)]) Tj\n15.25 0 Td\n(T) Tj\n7.1 0.85 Td\n(5QPQ4P4O44) Tj\n-467.5 -19.95 Td\n/g 9 Tf\n( !\"#$%&'\\(\"\\)*) Tj\n519.8 0 Td\n(+\\(,\"$%) Tj\n-23.2 -11.5 Td\n/c 8 Tf\n(-- \"0";
            int i = 0;
            var outputData = PdfArrayDataType.GetRawData(inputData, ref i);
            Assert.AreEqual("[(:)19(-)]", outputData);
        }

        [TestMethod]
        public void TestGetRawDataTextWithSquareBracketTest()
        {
            var inputData = "[(:])19(-)] TJ\n0 -13.65 Td\n(\"$$7.#8-8:;9-) Tj\n0 -14.7 Td\n(+) Tj\n[(8)75(\")75(89<9#8-6)(\")75(89-)] TJ\n144 29.2 Td\n(\"96) Tj\n0 -14.65 Td\n($.!!9#8) Tj\n[(-)31(\"$$7.#8)] TJ\n-65.6 -12.1 Td\n/c 7 Tf\n(=>?@\\)*-ABC-DE'>F) Tj\n17.55 12.75 Td\n(=>?@\\)*-&GH) Tj\n14.95 12.5 Td\n(IJKL\\)*) Tj\n-13.8 30 Td\n(=>?@\\)*-MN') Tj\n46.9 -56.85 Td\n/c 8 Tf\n(O5POQP4O45) Tj\n-144 40 Td\n(/ \"#-#7-) Tj\n144 0.85 Td\n(\"931-O42O-OR1S-4O41-13R3-5OQ) Tj\n0 14.75 Td\n(OR1S4O4113R35OQ) Tj\n-10.45 -15.6 Td\n(T) Tj\n0 -12.5 Td\n(T) Tj\n0 -14.65 Td\n(T) Tj\n0 -12.5 Td\n(T) Tj\n0.1 55.25 Td\n(T) Tj\n-77.05 -14.1 Td\n/c 7 Tf\n(UV'WK\\)*-=>?@J\\)-U\\)GX\\)*-MN'\\)*) Tj\nET\nQ\nBT\n1 0 0 1 240.35 -205.7 Tm\n0 0 0 sc\n/c 8 Tf\n(Y!7<-) Tj\n115.15 1.95 Td\n(ZK) Tj\n17.85 -0.25 Td\n(T) Tj\n9.6 1.1 Td\n(OQPO[P4O44) Tj\n63.7 -0.85 Td\n[(8)75(\\\\)] TJ\n23.85 0 Td\n(U\\)]) Tj\n15.25 0 Td\n(T) Tj\n7.1 0.85 Td\n(5QPQ4P4O44) Tj\n-467.5 -19.95 Td\n/g 9 Tf\n( !\"#$%&'\\(\"\\)*) Tj\n519.8 0 Td\n(+\\(,\"$%) Tj\n-23.2 -11.5 Td\n/c 8 Tf\n(-- \"0";
            int i = 0;
            var outputData = PdfArrayDataType.GetRawData(inputData, ref i);
            Assert.AreEqual("[(:])19(-)]", outputData);
        }


    }
}
