# Sample App for fields blacklisting/excluding with loggly supported dotnet libraries

This sample .NET C# console project helps you to blacklist or exclude some specific fields from your log events. Sometimes, there could be some situation where you want to restrict some fields from being logged to Loggly so in that case, you can refer to that demo project which can give you a basic understanding to achieve this use case. Please note that this project is using one of the Loggly's supported [.NET Serilog library](https://github.com/serilog/serilog-sinks-loggly).

**Note:** Please don't forget to replace your loggly customer token at line [#9](https://github.com/loggly/sources-sample-app/blob/master/dotnet/ExcludeFieldsNETSerilog/ExcludeFieldsNETSerilog/App.config#L9) of App.config file.

The implementation of fields blacklisting is generic and independent of any library so you can easily integrate the blacklisting code even with most popular [log4net-loggly](https://github.com/loggly/log4net-loggly) library too.
