<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminSide.aspx.cs" Inherits="VendingMachine.AdminSide" Async="true" AsyncTimeout="60" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Управление кофе-машиной</title>
    <script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    <script type="text/javascript">

        function LoadBeverage(selectedBeverage) {
            $.ajax({
                type: "POST",
                url: "/AdminSide.aspx/ShowBeverageInfo",
                data: JSON.stringify({ name: selectedBeverage }),
                contentType: "application/json; charset=utf-8",
                async: false,
                cache: false,
                dataType: "json",
                success: function (response) {
                    if (response != null && response.d != null) {
                        var data = response.d;
                        //we need to parse it to JSON
                        data = $.parseJSON(data);
                        alert(data.name);
                        alert(data.price);
                    }
                },
                error: function (req, status, error) {
                    alert("x" + error + status);
                }
            }).done(function (result) { });
        }

        function Add() {
            var newname = document.getElementById('newName').value;
            var newcount = document.getElementById('newCount').value;
            var newprice = document.getElementById('newPrice').value;

            if ((newname == "") || (newcount == "") || (newprice == "")) { alert("Введите все данные!") }
            else {
            $.ajax({
                type: "POST",
                url: "/AdminSide.aspx/AddNewBeverage",
                data: JSON.stringify({ name: newname, count: newcount, price: newprice }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response != null && response.d != null) {
                        var data = response.d;
                        data = $.parseJSON(data);
                        if (data == 1) { alert("Новый напиток добавлен!") }
                    }
                },
                error: function (req, status, error) {
                    alert("x" + error + status);
                }
            }).done(function (result) { });
            }
        }

        function Change() {

            //var changedName = document.getElementById('SelectBeverage').value;
            var changedName = "macchiato";
            var changedPrice = document.getElementById('changePrice').value;
            var changedCount = document.getElementById('changeCount').value;

            if ((changedPrice == "") || (changedCount == "")) { alert("Введите все данные!") }
            else {
                $.ajax({
                    type: "POST",
                    url: "/AdminSide.aspx/ChangeBeverageInfo",
                    data: JSON.stringify({ name: changedName, count: changedCount, price: changedPrice }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response != null && response.d != null) {
                            var data = response.d;
                            data = $.parseJSON(data);
                            if (data == 1) { alert("Изменения сохранены!") }
                        }
                    },
                    error: function (req, status, error) {
                        alert("x" + error + status);
                    }
                }).done(function (result) { });
            }
        }
    </script>

</head>
<body>

    <div class="container">
        <p>Выберите напиток, параметр которого хотите изменить (функция в доработке) </p>
        <form id="ShowBeverage" runat="server">

            <select id="SelectBeverage" multiple="false" runat="server" onchange="LoadBeverage(document.location=this.options[this.selectedIndex].value)">
            </select>
        </form>
    </div>

    <div>
        <h2>Внесите измененные данные </h2>
        <p id="showName" runat="server"> Название: </p>
        <p id="showPrice" runat="server"> Цена: </p>
        <input id="changePrice"/>
        <p id="showCount" runat="server">Количество: </p>
        <input id="changeCount"/>

        <button  onclick="Change()">Сохранить </button>
    </div>

    <div>
        <h2>Добавить новый напиток </h2>
        <p>Название: </p>
        <input id="newName"/>
        <p>Цена: </p>
        <input id="newPrice"/>
        <p>Количество: </p>
        <input id="newCount"/>
        <button onclick="Add()"> Добавить </button>
    </div>
</body>
</html>
