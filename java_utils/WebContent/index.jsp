<%@ page language="java" contentType="text/html; charset=utf-8"
	pageEncoding="utf-8"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
<title>我的主页</title>
<script type="text/javascript" src="js/jquery.js"></script>
<script type="text/javascript" src="js/jquery.crc.js"></script>
<script type="text/javascript" src="js/jquery.upload.js"></script>
<script type="text/javascript" src="js/jquery.tools.js"></script>
<script type="text/javascript" language="javascript">
	function checkfile() {
		var size = 0;
		var doc = document.getElementById("file1");
		if (doc && doc.files.length > 0) {
			size = doc.files[0].size;
		}
		if (size != $("#file1_size").text()) {
			$("#file1_size").text(size);
		}
		setTimeout(checkfile, 1000);
	}
	var d = null;
	function doUpload() {
		d = new amFileUpload({
			fileid : "file1",
			url : "http://127.0.0.1:8889",
			readsize : 1 * 1024,
			retry : 3,
			success : function(d) {
				alert(d);
			},
			error : function(msg) {
				alert(msg);
			},
			complete : function() {
				alert("complete");
			},
			process : function(pos, msg) {
				$("#showstatus").html(pos + "/" + d.filemaxsize + "---" + msg);
			},
			sign : function(d) {
				var crc = $.crc32.call(d);
				var crcs = $.tools.toHexString(crc);
				console.log("crc:"+crc+"   crcs :" + crcs)
				return crcs;//$.crc32.call(d).toString(16);
			}
		});
		d.start();
	}
	function doPause() {
		if (d != null) {
			d.pause();
		}
	}
	function doContinue() {
		if (d != null) {
			d.go();
		}
	}
	function getImage() {
		document.getElementById("imgyzm").src = "./getimage?t=" + Math.random();
	}
	function getImage1() {
		document.getElementById("imgyzm1").src = "./testServlet?t=" + Math.random();
	}
</script>
</head>
<body onload="checkfile();getImage();getImage1();">
	ssssssssssssssssssssssssssssss ${a}
	<br />
	<form action="upload.do" enctype="multipart/form-data" method="post">
		type： <input id="type" type="text" name="type" value="3" /><br />
		<div id="showstatus"></div>
		<br /> 上传文件1：<input type="file" id="file1" name="file1" /> <span
			id="file1_size">0</span><br /> <input type="button" value="提交"
			onclick="doUpload();" /> <input type="button" value="暂停/停止"
			onclick="doPause();" /> <input type="button" value="继续上传"
			onclick="doContinue();" />
		<hr />

		验证码:<img id="imgyzm" alt="验证码" src="#" /> <a href="#"
			onclick="javascript:getImage();">换一张</a><br />
		验证码1:<img id="imgyzm1" alt="验证码" src="#" /> <a href="#"
			onclick="javascript:getImage1();">换一张</a><br />

	</form>
</body>
</html>