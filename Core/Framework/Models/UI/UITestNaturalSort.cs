using System;
using System.Collections.Generic;
using System.Data;

namespace GrowthWare.Framework.Models.UI;

public class UITestNaturalSort
{
    public List<MColumns> DataTable {get; set;}
    public List<MColumns> DataView {get; set;}
    public string StartTime {get; set;}
    public string StopTime {get; set;}
    public string TotalMilliseconds {get; set;}
}

public class MColumns
{
    public string col1 {get; set;}
    public string col2 {get; set;}
}