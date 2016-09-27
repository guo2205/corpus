using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

public class ExcelHadle
{
    Application app;
    Workbooks wbks;
    _Workbook _wbk;
    Sheets shs;
    _Worksheet _wsh;


    /// <summary>
    ///构造函数需要填入EXCEL的路径
    /// </summary>
    /// <returns></returns>
    public ExcelHadle(string _path)
    {
        try
        {
            app = new Application();
            wbks = app.Workbooks;
            _wbk = wbks.Add(_path);

            //得第1个工作簿
            shs = _wbk.Sheets;
            _wsh = (_Worksheet)shs.get_Item(1);
        }
        catch
        {
        }
    }

    public void ChangeSheet(int _index)
    {
        _index = _index > 0 ? _index : 1;
        _wsh = (_Worksheet)shs.get_Item(_index);
    }

    public MyJson.JsonNode_Array GetData()
    {

        MyJson.JsonNode_Array jsArray = new MyJson.JsonNode_Array();

        //读取单元格子数据
        for (int i = 1; _wsh.Cells[i, 1].Value != null || !string.IsNullOrEmpty(_wsh.Cells[i, 1].Value); i++)
        {
            try
            {
                string data_code = _wsh.Cells[i, 1].Value.ToString();
                string data_q = _wsh.Cells[i, 2].Value.ToString();
                string data_a = _wsh.Cells[i, 3].Value.ToString();

                string[] questions = data_q.Split('\n');
                string[] answers = data_a.Split('\n');
                MyJson.JsonNode_Object json = new global::MyJson.JsonNode_Object();
                json.SetDictValue("id", data_code);
                json["f"] = GetJsonArray(questions);
                json["q"] = GetJsonArray(answers);
                jsArray.Add(json);
            }
            catch
            {

            }

        }
        return jsArray;
    }

    public void SetData(List<string> flist,List<string> qlist)
    {
        for (int i = 0; i < 10; i++)
        {
            _wsh.Cells[i, 1] = "1";
            _wsh.Cells[i, 2] = "2";
        }
        _wbk.Save();
        //return true;
    }
    MyJson.JsonNode_Array GetJsonArray(params object[] _str)
    {
        MyJson.JsonNode_Array myArray = new MyJson.JsonNode_Array();
        for (int i = 0; i < _str.Length; i++)
        {
            myArray.Add(new MyJson.JsonNode_ValueString(_str[i].ToString()));
        }
        return myArray;
    }
}
