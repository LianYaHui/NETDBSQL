using System;

namespace FoxzyDBSql
{
    /// <summary>
    /// 执行事物的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OnTranstionEvent(object sender, EventArgs e);


    /// <summary>
    /// 当输入参数为Dictionary<String,Object> 执行的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OnInputIsDictionaryEvent(object sender, DataParameterEventArgs e);

    /// <summary>
    /// 当输入参数为IEnumerable<IDataParameter>集合 执行的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OnInputIsListEvent(object sender, DataParameterEventArgs e);


    /// <summary>
    /// 当输入参数为Object 执行的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OnInputIsObjectEvent(object sender, DataParameterEventArgs e);
}
