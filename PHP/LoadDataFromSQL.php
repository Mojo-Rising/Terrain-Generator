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
	if($_POST["tile"]) {
		$tilename = $_POST["tile"];
	} else {
		echo "No POST tile received";
	}
	if($_POST["size"]) {
		$size = $_POST["size"];
	} else {
		echo "No POST tile size received";
	}
	if($_POST["lod"]) {
		$lod = $_POST["lod"];
	} else {
		echo "No POST LOD received";
	}

	$lodfactor = $lod / 2;

	$tsql = "SELECT t.[height]
		FROM
		(
		SELECT height, x, y, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rownum
		FROM [Limburg02].[dbo].[".$tilename."]
		) AS t
		WHERE
		(
		 (x % ".$lodfactor." = 0)
		 AND
		 (y % ".$lodfactor." = 0)
		)";

	$getResults = $conn->prepare($tsql);
	$getResults->execute();
	$results = $getResults->fetchAll(PDO::FETCH_BOTH);

	$count = 0;

	foreach($results as $row) {
		echo $row['height'];
		if($count != 1050624) {
			echo ",";
		}
		$count++;
	}
	$mb = memory_get_usage() / 1048576;

	/*
	SELECT t.[height]
		FROM
		(
		SELECT height, x, y, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rownum
		FROM [Limburg].[dbo].[".$tilename."]
		) AS t
		WHERE
		(
		 (x % 2 = 0)
		 AND
		 (y % 2 = 0)
		)
		 */

	?>

