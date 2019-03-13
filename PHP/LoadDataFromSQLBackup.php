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
	$tsql = "SELECT t.[height]
		FROM
		(
		SELECT height, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rownum
		FROM [Limburg].[dbo].[".$tilename."]
		) AS t
		WHERE
		(
		 (t.rownum % 4 = 0)
		 OR
		 (t.rownum = 0)
		 OR
		 (t.rownum = 4198401)
		)";
	$getResults = $conn->prepare($tsql);
	$getResults->execute();
	$results = $getResults->fetchAll(PDO::FETCH_BOTH);

	foreach($results as $row) {
		echo $row['height'];
		echo ",";
	}
	$mb = memory_get_usage() / 1048576;

	?>

SELECT t.[height]
FROM
(
SELECT height, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rownum
FROM [Limburg].[dbo].[".$tilename."]
) AS t
WHERE t.rownum % 4 = 0







SELECT t.[height]
FROM
(
SELECT height, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rownum
FROM [Limburg].[dbo].".$tilename."]
) AS t
WHERE
(
 (t.rownum % 4 = 0)
 OR
 (t.rownum = 0)
 OR
 (t.rownum = 4198401)
 )