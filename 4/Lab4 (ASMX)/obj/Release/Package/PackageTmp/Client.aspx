<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Client.aspx.cs" Inherits="Lab4_ASMX.Client" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Client form</title>
    <script>
        async function Send()
        {
            const data =
            {
                x: document.getElementById("x").value,
                y: document.getElementById("y").value
            };
            const response = await fetch(`Simplex.asmx/AddMessageJSON`,
                {
                    method: 'POST',
                    headers:
                    {
                        'Content-Type': 'application/json;'
                    },
                    body: JSON.stringify(data)
                }
            );
            const dataR = await response.json();

            if (!response.ok)
            {
                throw new Error(dataR.message || 'Something wrong');
            }
            else
            {
                document.getElementById("result").innerHTML = `${dataR.d}`;

            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <input type="text" id="x"/>
                <input type="text" id="y"/>
                <input type="button" onclick="Send()" value="Send" />
            </div>
            <div>
                <h1 id="result"> </h1>
            </div>
        </div>
    </form>
    
</body>
</html>
