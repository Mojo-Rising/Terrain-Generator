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

	//$counter = 0;

	

    $log_directory = "tiles";

    foreach(glob($log_directory.'/*') as $file) {

    	print("we have a file: ");
    	print("http://127.0.0.1/test/".$file);

	    $string = file_get_contents("http://127.0.0.1/test/".$file);
		$json_a = json_decode($string, true);

		$coord = "_x".$json_a['X']."_y".$json_a['Y'];
		$tsql = "CREATE TABLE [".$coord."]([x] [int], [y] [int], [height] [float] NULL) ON [PRIMARY]";
		$runSQLCreateTable = $conn->prepare($tsql);
		$runSQLCreateTable->execute();

		$jsonIterator = new RecursiveIteratorIterator(
	    new RecursiveArrayIterator(json_decode($string, TRUE)),
	    RecursiveIteratorIterator::SELF_FIRST);

	    $localY = 0;
	    $localX = 0;
	    $globalX = 0;

	    $values = '';

		foreach ($jsonIterator as $key => $val) {

		    if(!is_array($val) && is_float($val)) {

		    	$localX = $globalX - ($localY * 2049);

		    	//$parsedval = number_format((float)$val, 2, '.', '');
		    	$parsedval = $val;

		        //$query = "INSERT [".$coord."] ([x], [y], [height]) VALUES (".$localX.", ".$localY.", ".$parsedval.")";

		        $values .= "('{$localX}', '{$localY}', '{$parsedval}'),";		        

				$globalX++;


				if($localX % 512 == 0 && $localX != 0) {
					$values = rtrim($values, ',');
		    		$query = "INSERT [Limburg02].[dbo].[".$coord."] ([x], [y], [height]) VALUES {$values} ;";

		    		$runSQLAddData = $conn->prepare($query);
					$runSQLAddData->execute();

					//print("{$query} <br>");

					$values = '';
				}

		    } else if(is_array($val) && $key != 0) {

				$localY++;
			
				print("Y: ".$localY.", ");
			}

		    
		    
		}

				    
	}

	

	//echo "number of numbers: $counter";

	//echo $query;

	


?>