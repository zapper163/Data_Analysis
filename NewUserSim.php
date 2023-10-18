<?php
if ($_SERVER["REQUEST_METHOD"] === "POST") {
    if (isset($_POST["name"])) {
        $name = $_POST["name"];
    } else {
        $name = "null";
    }

    if (isset($_POST["country"])) {
        $country = $_POST["country"];
    } else {
        $country = "null";
    }

    if (isset($_POST["date"])) {
        $date = $_POST["date"];
    } else {
        $date = "null";
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

    // Consulta a la base de datos
    $sql = "INSERT INTO Users(Name, Sex, Country) VALUES ('$name', 0, '$country')";
    $resultado = $conn->query($sql);
    $last_id = $conn->insert_id;
    echo $last_id;

    // Cerrar la conexión cuando hayas terminado
    $conn->close();

}
else {
    echo "nullData";
}
?>