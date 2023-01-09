using CPMS_PDFReport_Marker;
using iText.Kernel.Pdf;
using Newtonsoft.Json;


try
{
    string originalFilePath = CPMS_PDFReport_Marker.Const.OriginFilePath;
    string originalFileName = GetOriginalFileName();
    string successFileName = GetSuccessFileName();

    var _fileTool = new FileTool();
    var _markerTool = new MarkerTool();

    //取得原檔
    string readFileErrorMsg;
    bool readFileStatus;
    var originalFile = _fileTool.ReadFile(originalFilePath, originalFileName, out readFileErrorMsg, out readFileStatus);

    if (!readFileStatus)
    {
        Console.WriteLine($"【失敗】{readFileErrorMsg}");
        return;
    }

    string markerString = GetMarker();
    bool getPDFFontStatus;
    string getPDFFontErrorMsg;
    var fontString = _markerTool.GetPDFFont(out getPDFFontStatus, out getPDFFontErrorMsg);

    if (!getPDFFontStatus)
    {
        Console.WriteLine($"【失敗】{getPDFFontErrorMsg}");
        return;
    }

    string makeNewPDFFileWithMarkerErrorMsg;
    bool makeNewPDFFileWithMarkerStatus;

   var fileMakeResult = _fileTool.MakeNewPDFFileWithMarker(out makeNewPDFFileWithMarkerErrorMsg, originalFile, fontString, successFileName, CPMS_PDFReport_Marker.Const.SuccessFilePath,markerString);

    if (!fileMakeResult)
    {
        Console.WriteLine(makeNewPDFFileWithMarkerErrorMsg);
    }

    Console.ReadKey();

}
catch (Exception ex)
{

    throw;
}




static string GetSuccessFileName()
{
    Console.WriteLine("請輸入欲產出之檔案檔名");
    var strProgram = System.Console.ReadLine();
    return strProgram;
}

static string GetOriginalFileName()
{
    Console.WriteLine("請輸入欲加浮水印之原檔名");
    var strProgram = System.Console.ReadLine();
    return strProgram;
}

static string GetMarker()
{
    Console.WriteLine("請輸入愈加入之浮水印字串");
    var strProgram = System.Console.ReadLine();
    return strProgram;
}