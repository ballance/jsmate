const fadeInterval = 150;

function setVersion() {
	try	{
		var ver = JSON.parse(readVersion('version.json')).version;
		var versionLabel = document.getElementById("versionLabel");
		versionLabel.innerHTML = ver;
	}
	catch (e)
	{
		$('statuss').append('<br />' + e);
	}
}

function readVersion(file)
{
	var versionRawFound = '';
    var versionRaw = new XMLHttpRequest();
    versionRaw.open("GET", file, false);
    versionRaw.onreadystatechange = function ()
    {
        if(versionRaw.readyState === 4)
        {
            if(versionRaw.status === 200 || versionRaw.status == 0)
            {
                versionRawFound = versionRaw.responseText;
            }
        }
    }
    versionRaw.send(null);

    return versionRawFound;
}

function createGuid()
{
	var guid = 'zzzzzzzz-zzzz-4zzz-tzzz-zzzzzzzzzzzz'.replace(/[zt]/g, function(c) {
    	var r = Math.random()*16|0, v = c == 'z' ? r : (r&0x3|0x8);
    	return v.toString(16);
	});
	return guid;
}

// Extend jQuery to provide flash UX on pieces
jQuery.fn.extend({
  flashClick: function() {
    return this.fadeIn(fadeInterval).fadeOut(fadeInterval).fadeIn(fadeInterval);
    }});
  

function setBoardInitial()
{
	// White

	// Rooks
	$(".block.row8.col1").html('&#9814');
	$(".block.row8.col1").click(function() { $('#statuss').append('<br />clicked white rook 1');});
	$(".block.row8.col8").html('&#9814');
	$(".block.row8.col8").click(function() { $('#statuss').append('<br />clicked white rook 2');});

	// Knights
	$(".block.row8.col2").html('&#9816;');
	$(".block.row8.col2").click(function() { $('#statuss').append('<br />clicked white knight 1');});
	$(".block.row8.col7").html('&#9816;');
	$(".block.row8.col7").click(function() { $('#statuss').append('<br />clicked white knight 2');});


	// Knights
	$(".block.row8.col3").html('&#9815;');
	$(".block.row8.col3").click(function() { $('#statuss').append('<br />clicked white bishop 1');});
	$(".block.row8.col6").html('&#9815;');
	$(".block.row8.col6").click(function() { $('#statuss').append('<br />clicked white bishop 2');});

	// Queen
	$(".block.row8.col4").html('&#9813;');
	$(".block.row8.col4").click(function() { $('#statuss').append('<br />clicked the white queen');});

	// King
	$(".block.row8.col5").html('&#9812;');
	$(".block.row8.col5").click(function() { $('#statuss').append('<br />clicked the white king');});
	
	// Pawns
	$(".block.row7.block").html('&#9817;');
	$(".block.row7.block").click(function() { 
		$('#statuss').append('<br />clicked a white pawn');
	});

	// Black

	// Rooks
	$(".block.row1.col1").html('&#9820');
	$(".block.row1.col1").click(function() { $('#statuss').append('<br />clicked black rook 1');});
	$(".block.row1.col8").html('&#9820');
	$(".block.row1.col8").click(function() { $('#statuss').append('<br />clicked black rook 2');});
	
	// Bishops
	$(".block.row1.col2").html('&#9822;');
	$(".block.row1.col2").click(function() { $('#statuss').append('<br />clicked black knight 1');});
	$(".block.row1.col7").html('&#9822;');
	$(".block.row1.col7").click(function() { $('#statuss').append('<br />clicked black knight 2');});

	// Knights
	$(".block.row1.col3").html('&#9821;');
	$(".block.row1.col3").click(function() { $('#statuss').append('<br />clicked black bishop 1');});
	$(".block.row1.col6").html('&#9821;');
	$(".block.row1.col6").click(function() { $('#statuss').append('<br />clicked black bishop 2');});

	// King
	var blackKing = $(".block.row1.col5");
	blackKing.html('&#9818;');
	blackKing.click(function() { $('#statuss').append('<br />clicked black king'); blackKing.flashClick();});
	
	// Queen
	var blackQueen = $(".block.row1.col4");
	blackQueen.html('&#9819;');
	blackQueen.click(function() { $('#statuss').append('<br />clicked black queen'); blackQueen.flashClick();});
	
	// Pawns
	$(".block.row2.block").html('&#9823;');
	$(".block.row2.block").click(function() { 
		$('#statuss').append('<br />clicked a black pawn');
	});
}

function setBoardCookie() {
	//$('#statuss').append('<br />start');
	
	if (document.cookie.replace(/(?:(?:^|.*;\s*)instanceCookie\s*\=\s*([^;]*).*$)|^.*$/, "$1") !== "true") {
    	var newGuid = createGuid();
    	document.cookie = "instanceCookie=true; expires=Fri, 31 Dec 9999 23:59:59 GMT";
		document.cookie = "boardId=" + newGuid + "; expires=Fri, 31 Dec 9999 23:59:59 GMT";
		//$('#statuss').append('<br />wrote boardid cookie with: ' + newGuid);
	}
	else
	{
		//$('#statuss').append('<br />nope');
	}
	//$('#statuss').append('<br />end');
}

function clearBoardCookie() {
	document.cookie = "instanceCookie=; expires=Fri, 31 Dec 9999 23:59:59 GMT";
	document.cookie = "boardId=; expires=Fri, 31 Dec 9999 23:59:59 GMT";
}

function readBoardIdFromCookie() {
	var currentBoardId = document.cookie.replace(/(?:(?:^|.*;\s*)boardId\s*\=\s*([^;]*).*$)|^.*$/, "$1");
	//$('#statuss').append('<br />read id from cookie: ' + currentBoardId);
	return currentBoardId;
}

function retrieveBoardState()
{
	// Get this jQuery madness out of here if time permits.  Plain Jane JS is more than sufficient
	var boardId = readBoardIdFromCookie();
	$('#statuss').append('<br />Attempting to get board: [' + boardId + ']');
	
	var apiUri = 'http://localhost:9997/board/' + boardId;
	
	$.getJSON(apiUri)
		.done(function(data) {
			try
			{
				$('#statuss').append('<br />Success! Retrieved board: [' + data.Id + ']');
			}
			catch(e)
			{
				$('#statuss').append('<br />Failed to deserialize json board')
			}
		})
		.fail(function() {
			$('#statuss').append('<br /><p>API call to ' + apiUri + ' failed</p>');

		})
		.always(function() {
			//$('#statuss').append('<br /><p>API call completed as promised</p>');	
			//$('#statuss').append('<br />API promise completed');		
		})
}

function clearStatus()
{
	$('#statuss').html('');
}

function wireUpButtons()
{
	$('#refreshboard').click(function() {
		retrieveBoardState();
	});


	$('#clearStatus').click(function() {
		clearStatus();
	});

	$('#newBoard').click(function() {
		clearBoardCookie();
		clearStatus();
		setBoardCookie();
		retrieveBoardState();
	});

	$('#movePawn').click(function() {

		var boardId = readBoardIdFromCookie();
	
		var apiUri = 'http://localhost:9997/move/' + boardId + '/1/Queen';
		$('#statuss').append('<br />moving piece: ' + apiUri);

		$.getJSON(apiUri)
			.done(function(data) {
				try
				{
					$('#statuss').append('<br />moved board: ' + data);
				}
				catch(e)
				{
					$('#statuss').append('<br />failed to deserialize json move')
				}
			})
			.fail(function() {
				$('#statuss').append('<br /><p>API call to ' + apiUri + ' failed</p>');

			})
			.always(function() {
				//$('#statuss').append('<br /><p>API call completed as promised</p>');	
				//$('#statuss').append('<br />API promise completed');		
			})

	});


}

$(document).ready(function() {
	wireUpButtons();

	setVersion();
	setBoardInitial();

	//clearBoardCookie();
	setBoardCookie();

	retrieveBoardState();

});