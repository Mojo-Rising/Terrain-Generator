<?php
	try
	{
		$conn = new PDO("sqlsrv:Server=DESKTOP-BLQ3M1P\SQLEXPRESS;Database=Limburg02");
		$conn->setAttribute( PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

	}

	catch(Exception $e)
	{
		die (print_r($e->getMessage()));
	}


	$tsql = "SELECT TABLE_NAME 
		FROM Limburg02.INFORMATION_SCHEMA.TABLES 
		WHERE TABLE_TYPE = 'BASE TABLE'";
	$getResults = $conn->prepare($tsql);
	$getResults->execute();
	$results = $getResults->fetchAll(PDO::FETCH_BOTH);

	$count = 0;
	$numItems = count($results);

	foreach($results as $key=>$row) {
		echo $row['TABLE_NAME'];
		if(++$count != $numItems) {
			echo ",";
		}
		//$count++;
	}


?>