NETDBSQL
========

C#操作数据库的方法合集

 配置 连接 字符串 

SqlManageUtil.ConncetionString = "";//这里自行配制


实例化：
SqlManageUtil db = new SqlManageUtil();


先进行简单的select 操作，从tb表中查询出所有数据
String sql = db.CreateSelect().From("tb").Select().ToSql();
最后执行ToSql 转化为相应的Sql语句。
即sql="select tb.* from tb";


String sql = db.CreateSelect().From("tb").Select("ID,Name as name,Age").ToSql();

语句很简单，从tb表中Select 出 ID,Name和Age 三列，


在实际的操作中Select 操作一般都返回DataSet,所以在应用中换成.ToDataSet();
直接返回DataSet对象，


当然，示例的中返回的Sql = "select ID,Name as name,Age from tb"


带Where 的Select :


var sql = db.CreateSelect().From("tb")
 .Select("ID,Name as name,Age")
 .Where("Age=20 and Name like *%张*")
 .ToSql();


输出Sql为”select ID,Name as name,Age from tb where Age=20 and Name like *%张*“


当然很多时候查询都是要根据UI上的控件值，所以Select 一定要参数化，这里我们用SetParameter，有三种重载方式
public abstract AbsDbExpression SetParameter(IEnumerable<SqlParameter> pars);
 public abstract AbsDbExpression SetParameter(params SqlParameter[] pars);
 public abstract AbsDbExpression SetParameter(string replaceText, object value);


demo 1:
 var sql = db.CreateSelect().From("tb")
 .Select("ID,Name as name,Age")
 .Where("Age>@age and Name = @name")
 .SetParameter("@age",10)
 .SetParameter("@name","张三")
 .ToSql();
demo 2:
 var sql = db.CreateSelect().From("tb")
 .Select("ID,Name as name,Age")
 .Where("Age>@age and Name = @name")
 .SetParameter(new SqlParameter("@name","张三"),new SqlParameter("@age",20))
 .ToSql();


SetParameter 方法是将sql 参数化，不管select ，update,insert 还是delete都要调用的，这点数据ado.net 的同学都知道 ，我就不多说了





连接查询也是项目中比较常用的查询：
var sql = db.CreateSelect().From("tb","t")
 .Select()
 .InnerJoin("tb2 as t2")
 .On("t2.UserID = t.ID")
 .ToSql();


sql=
 select t.*, t2.* from tb as t inner join tb2 as t2 on t2.UserID = t.ID
类似的我们还可以
var sql = db.CreateSelect().From("tb","t")
 .Select("t.Name,t2.Date as d")
 .Where("t.ID=2 and t2.Sex=@sex")
 .InnerJoin("tb2 as t2")
 .On("t2.UserID = t.ID")
 .SetParameter("@sex",*男*)
 .ToSql();


输出sql=
 select t.Name,t2.Date as d from tb as t inner join tb2 as t2 on t2.UserID = t.ID where t.ID=2 and t2.Sex=@sex

其他连接，如left join ,right join 将代码改为LeftJoin ,RightJoin