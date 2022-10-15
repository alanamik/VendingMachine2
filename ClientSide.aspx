
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientSide.aspx.cs" Inherits="VendingMachine.ClientSide" Async="true" AsyncTimeout="60" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Styles/StyleSheet.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
    <title></title>
    <script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    <script type="text/javascript">

        let insertedCoins = 0;
        let selectedBeverage = "";

        function AddCoin(coin) {
            insertedCoins = insertedCoins + coin;
            ShowInsertedCoin();
        }

        function ShowInsertedCoin() {
            var showCoins = document.getElementById('insertedCoins');
            showCoins.textContent = "Вы внесли " + insertedCoins;
        }
        function SelectBeverage(beverage) {          
            selectedBeverage = beverage;
            ShowSelectedBeverage();
        }

        function ShowSelectedBeverage() {
            var showBeverage = document.getElementById('selectBeverage');
            var nameBeverage;

            if (selectedBeverage == 'blackCoffee') { nameBeverage = 'черный кофе' }
            if (selectedBeverage == 'espresso') { nameBeverage = 'эспрессо' }
            if (selectedBeverage == 'cappuccino') { nameBeverage = 'капучино' }
            if (selectedBeverage == 'macchiato') { nameBeverage = 'макиато' }
            if (selectedBeverage == 'coffeeWithCream') { nameBeverage = 'кофе со сливками' }

            showBeverage.textContent = "Вы выбрали " + nameBeverage;
        }

        function Pay() {
            $.ajax({
                type: "POST",
                url: "/ClientSide.aspx/Sell",
                data: JSON.stringify({ coins: insertedCoins, nameOfBeverage: selectedBeverage }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response != null && response.d != null) {
                        var data = response.d;
                        //alert(typeof (data)); //it comes out to be string 
                        //we need to parse it to JSON 
                        data = $.parseJSON(data);
                        if (data == -1) {  if (!alert("Внесена недостаточная сумма денег!")) { window.location.reload(); } }
                        else { { if (!alert("Наслаждайтесь кофе! Ваша сдача: " + data)) { window.location.reload(); } } }
                        
                    }
                },
                error: function (req, status, error) {
                    alert("x" + error + status);
                }
            }).done(function (result) { })}
    </script>
</head>
<body>
    <div>
        <h1>Coffee machine</h1>
    </div>
    <div>
        <div>
            <p>Внесите монеты: </p>
            <div class="outer">
                    <a class="inner" onclick="AddCoin(1)">1 </a>
                    <a class="inner" onclick="AddCoin(2)">2 </a>
                    <a  class="inner"  onclick="AddCoin(5)">5 </a>
                    <a  class="inner"  onclick="AddCoin(10)">10 </a>             
            </div>
        </div>
    </div>

    <div>
        <p>Выберите напиток: </p>
        <div>
            <a>
                <img id="blackCoffee" src="blackCoffee.jpg" alt="" onclick="SelectBeverage('blackCoffee')" runat="server"  />
                <p id="blackCoffee_price" runat="server" >price</p>
            </a>
            <a>
                <img id="espresso"  src="espresso.jpg" alt="" onclick="SelectBeverage('espresso')" runat="server"  />
                <p id="espresso_price" runat="server"  >price</p>
            </a>
            <a>
                <img id="cappuccino"  src="cappuccino.jpg" alt="" onclick="SelectBeverage('cappuccino')" runat="server"  />
                <p id="cappuccino_price" runat="server" >price</p>
            </a>
            <a>
                <img id="macchiato"  src="macchiato.jpg" alt="" onclick="SelectBeverage('macchiato')" runat="server"  />
                <p id="macchiato_price" runat="server" >price</p>
            </a>
            <a>
                <img id="coffeeWithCream" src="coffeeWithCream.jpg" alt="" onclick="SelectBeverage('coffeeWithCream')" runat="server"  />
                <p id="coffeeWithCream_price" runat="server" >price</p>
            </a>
        </div>

    </div>

    <div>
        <div>
            <p id="selectBeverage"></p>
        </div>
        <div>
            <p id="insertedCoins"></p>
        </div>
        <div>
            <button onclick="Pay()">Купить </button>
        </div>
    </div>
</body>
</html>
