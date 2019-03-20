## Seq v1.0.0

To produce an platform agnostic dll use:
`dotnet publish -c Release --self-contained false`

Then navigate to \bin\Release\netcoreappX.Y\publish
In there is a `seq.dll` you can execute it using
`dotnet seq.dll` or `dotnet seq.dll apply` if you want to immediately push the changes.