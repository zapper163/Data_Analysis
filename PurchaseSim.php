<?php
if ($_SERVER["REQUEST_METHOD"] === "POST") {
    if (isset($_POST["userId"])) {
        $userId = $_POST["userId"];
    } else {
        $userId = "null";
    }

    if (isset($_POST["itemId"])) {
        $itemId = $_POST["itemId"];
    } else {
        $itemId = "null";
    }

    if($userId == "null" || $itemId == "null"){
        die("Value error!");
    }

    $servername = "localhost"; // Cambia a la dirección del servidor si es diferente
    $username = "rubenaa3";
    $password = "n65quXrT3TQB";
    $database = "rubenaa3";

    // Crear una conexión
    $conn = new mysqli($servername, $username, $password, $database);

    // Verificar la conexión
    if ($conn->connect_error) {
    die("Error de conexión: " . $conn->connect_error);
    }
    
    $getPrice = "SELECT price FROM ItemsInfo WHERE item_id = $itemId";
    $resultado = $conn->query($getPrice);
    $money = mysqli_fetch_array($resultado)[0];
    // while ($row = mysqli_fetch_array($resultado)) echo $row[0];

    // Consulta a la base de datos
    $sql = "UPDATE Users SET Spent_Money = Spent_Money + $money WHERE UserID = $userId";
    $resultado = $conn->query($sql);
    echo $money;

    // Cerrar la conexión cuando hayas terminado
    $conn->close();

}
else {
    echo "nullData";
}
?>