<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientSide.aspx.cs" Inherits="VendingMachine.ClientSide" Async="true" AsyncTimeout="60" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title></title>
    <script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    <link href="~/Styles/StyleSheet.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
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
                        data = $.parseJSON(data);
                        if (data == -1) { if (!alert("Внесена недостаточная сумма денег!")) { window.location.reload(); } }
                        else { { if (!alert("Наслаждайтесь кофе! Ваша сдача: " + data)) { window.location.reload(); } } }

                    }
                },
                error: function (req, status, error) {
                    alert("x" + error + status);
                }
            }).done(function (result) { })
        }
    </script>
</head>
<body>
    <div>
        <h1>Coffee machine</h1>
    </div>      
        <h2>Внесите монеты: </h2>
            
    <div class="btnCoins-group">

         <br />
                <a class="btn btn-info" onclick="AddCoin(1)">1</a>
                <a class="btn btn-info" onclick="AddCoin(2)">2</a>
                <a class="btn btn-info" onclick="AddCoin(5)">5</a>
                <a class="btn btn-info" onclick="AddCoin(10)">10</a>
   </div>
    <br />
    <br />
    <br />

 <div>  <br /><h2>Выберите напиток: </h2>   <br /></div>

   <br />    
    
    <div class="container">

        <div class="row-coffeeImages">
            <div class="column-coffeeImage">
                <a>
                    <img id="blackCoffee" src="blackCoffee.jpg" alt="" onclick="SelectBeverage('blackCoffee')" runat="server" />
                    <p id="blackCoffee_price" runat="server">price</p>
                </a>
            </div>
            <div class="column-coffeeImage">
                <a>
                    <img id="espresso" src="espresso.jpg" alt="" onclick="SelectBeverage('espresso')" runat="server" />
                    <p id="espresso_price" runat="server">price</p>
                </a>

            </div>
            <div class="column-coffeeImage">
                <a>
                    <img id="cappuccino" src="cappuccino.jpg" alt="" onclick="SelectBeverage('cappuccino')" runat="server" />
                    <p id="cappuccino_price" runat="server">price</p>
                </a>
            </div>

            <div class="column-coffeeImage">
                <a>
                    <img id="macchiato" src="macchiato.jpg" alt="" onclick="SelectBeverage('macchiato')" runat="server" />
                    <p id="macchiato_price" runat="server">price</p>
                </a>
            </div>

            <div class="column-coffeeImage">
                <a>
                    <img id="coffeeWithCream" src="coffeeWithCream.jpg" alt="" onclick="SelectBeverage('coffeeWithCream')" runat="server" />
                    <p id="coffeeWithCream_price" runat="server">price</p>
                </a>
            </div>
        </div>
    </div>

    <br />
    <p></p>
    <div class="containerPay" > 
         <br />
    <p></p>
            <p id="selectBeverage"></p>
      <br />
            <p id="insertedCoins"></p>
     <br />
            <button onclick="Pay()">Купить </button>     
    </div>
</body>
</html>
