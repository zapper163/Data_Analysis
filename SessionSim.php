<?php
if ($_SERVER["REQUEST_METHOD"] === "POST") {
    if (isset($_POST["sessionId"])) {
        $sessionId = $_POST["sessionId"];
    } else {
        $sessionId = "null";
    }

    if (isset($_POST["userId"])) {
        $userId = $_POST["userId"];
    } else {
        $userId = "null";
    }

    if (isset($_POST["date"])) {
        $date = $_POST["date"];
    } else {
        $date = "null";
    }

    if($sessionId == "null" || $userId == "null" || $date == "null"){
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
    
    if ($sessionId == "startSession")
    {
        $sql = "INSERT INTO Sessions(UserID, Date) VALUES ('$userId', '$date')";

        $resultado = $conn->query($sql);
        $session_id = $conn->insert_id;
        echo $session_id;
    }
    else
    {
        $getStartDate = "SELECT Date FROM Sessions WHERE SessionID = $sessionId";
        $resultado = $conn->query($getStartDate);
        //$startDate = mysqli_fetch_array($resultado)[0];
        while ($row = mysqli_fetch_array($resultado)) $startDate = $row[0];

        $getDuration = "SELECT TIMESTAMPDIFF(SECOND, $startDate, $date)";
        $resultado = $conn->query($getDuration);
        $duration = mysqli_fetch_array($resultado)[0];
        $sql = "UPDATE Sessions SET Duration = $duration";

        $resultado = $conn->query($sql);
    }

    
    // Cerrar la conexión cuando hayas terminado
    $conn->close();

}
else {
    echo "nullData";
}
?>