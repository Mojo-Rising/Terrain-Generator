<?php
	try
	{
		$conn = new PDO("sqlsrv:Server=DESKTOP-BLQ3M1P\SQLEXPRESS;Database=Limburg");
		$conn->setAttribute( PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

	}

	catch(Exception $e)
	{
		die (print_r($e->getMessage()));
	}

	// $tsql = "SELECT * FROM dbo.Route";
	// $getResults = $conn->prepare($tsql);
	// $getResults->execute();
	// $results = $getResults->fetchAll(PDO::FETCH_BOTH);

	// foreach($results as $row) {
	// 	echo $row['Height'];
	// 	echo '<br>';
	// }

	$string = file_get_contents("http://127.0.0.1/test/Tile%20(5_5).json");
	$json_a = json_decode($string, true);

	$coord = "_x".$json_a['X']."_y".$json_a['Y'];

	$tsql = "CREATE TABLE [".$coord."]([height] [float] NULL) ON [PRIMARY]";
	$runSQLCreateTable = $conn->prepare($tsql);
	$runSQLCreateTable->execute();

	//$counter = 0;

	$jsonIterator = new RecursiveIteratorIterator(
    new RecursiveArrayIterator(json_decode($string, TRUE)),
    RecursiveIteratorIterator::SELF_FIRST);

	foreach ($jsonIterator as $key => $val) {
	    if(!is_array($val) && is_float($val)) {
	        $query = "INSERT [".$coord."] ([height]) VALUES (".$val.")";

	        $runSQLAddData = $conn->prepare($query);
			$runSQLAddData->execute();
	    } else{
	        //echo "$key => $val\n";
	    }
	}

	//echo "number of numbers: $counter";

	echo $tsql;

	


?>