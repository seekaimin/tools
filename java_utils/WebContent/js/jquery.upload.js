/**
文件分割上传
@author am
@date 2017-01-04
@update 
    2017-10-25 
    增加数据签名功能 新增方法 string sign(byte[] buffer)  可以不传
@param 
必填参数 {
url : 提交地址, 
fileid : 文件域ID id与name必须保持一致 
}
@param 
可选参数 {
readsize : 每次读取大小 默认值1K, 
retry : 失败时重试次数 默认值 3次, 
success : 成功时执行回调函数, 
error : 失败时执行回调函数, 
complete : 完成时执行回调函数, 
process : 提示传输进度 , 
sign : 数据签名 2017-10-25 
} 
@remark 能处理的服务端返回结构 
1  {"flag":0,"fileserverid":"服务端返回文件标识"} 
2  {"flag":0,"fileserverid":"服务端返回文件标识"}@
*/
(function () {

    // 必填参数
    // {
    // url : 提交地址,
    // fileid : 文件域ID id与name必须保持一致
    // }
    // 可选参数
    // {
    // readsize : 每次读取大小 默认值1K
    // retry : 失败时重试次数 默认值 3次
    // success : 成功时执行回调函数
    // error : 失败时执行回调函数
    // complete : 完成时执行回调函数
    // process : 提示传输进度
    // }

    var amFileUpload = function (opts) {
        var $this = this;
        $this.url = opts.url || undefined;
        var method = opts.method || "post";
        // 允许上传的文件类型
        $this.exts = opts.exts || [];
        // 文本域ID 不允许为空
        $this.fileid = opts.fileid || undefined;
        // blob size def 1024(B)
        $this.fileblob = opts.readsize || 1024;
        /**
        * 出现错误重试上传的最大次数
        */
        $this.RETRY_MAX_TIMES = 3;
        if (opts.retry != undefined) {
            $this.RETRY_MAX_TIMES = opts.retry;
        }
        // toUpperCase()
        // toLowerCase()
        $this.method = method.toUpperCase();

        var doc = document.getElementById($this.fileid);
        // 需要上传的文件
        $this.file = (doc && doc.files.length > 0) ? doc.files[0] : null;
        // 上传的文件名称
        $this.filename = doc ? doc.value : "";
        // 服务端返回的全路径文件名称
        $this.fileserverid = "";
        // 文件大小
        $this.filemaxsize = $this.file ? $this.file.size : 0;
        // 当前正在上传文件的开始位置
        $this.filestart = 0;

        // 本次发送的blod
        $this.blob = 0;
        /**
        * 储存当前上传状态值
        */
        $this.status = 1;
        /**
        * 存储当前消息
        */
        $this.message = "";
        /**
        * 执行成功
        * 
        */
        $this.success = opts.success;
        /**
        * 执行错误
        * 
        */
        $this.error = opts.error;
        /**
        * 执行完成
        * 
        */
        $this.complete = opts.complete;
        /**
        * 数据签名
        * 
        * @param buffer
        *            需要上传的数据
        */
        $this.sign = opts.sign;
        /**
        * 运行进程
        * 
        * @param pos
        *            文件传输进度
        * @param msg
        *            传输过程传入的消息
        */
        $this.process = opts.process;

        /**
        * 重置文件读取计数器
        */
        $this.reset = function () {
            $this.status = $this.STATUS_PENDING;
            $this.filestart = 0;
            $this.isPause = false;
        }
        /**
        * 检核上传文件是否为允许上传的文件类型
        */
        $this.checkExtend = function () {
            if ($this.exts.length == 0) {
                return true;
            }
            var index = $this.filename.lastIndexOf(".") + 1;
            var ext = $this.filename.substring(index).toUpperCase();
            for (var i = 0; i < $this.exts.length; i++) {
                if ($this.exts[i].toUpperCase() == ext) {
                    return true;
                }
            }
            // alert("file type error!")
            return false;
        }
        /**
        * 获取本次上传文件长度 0:已经上传完毕
        * 
        * @returns blob 当前应该读取文件的长度
        */
        $this.getReadBlob = function () {
            var blob = 0;
            if ($this.filemaxsize <= $this.filestart) {
                // 上传完成
            } else if ($this.filemaxsize - $this.filestart > $this.fileblob) {
                blob = $this.fileblob;
            } else {
                blob = $this.filemaxsize - $this.filestart;
            }
            return blob;
        }

        /**
        * 定义ajax请求对象
        */
        $this.xhr = null;

        /**
        * 处理返回数据
        */
        $this.handler = function () {
            var flag = 1;
            try {
                var response = this.responseText;
                $this.message = response;
                if (this.status == 200) {
                    var json = {};
                    if (response.indexOf('@') >= 0) {
                        // 包含@
                        var strs = response.split('@');
                        response = strs[0];

                    }
                    json = jQuery.parseJSON(response);
                    flag = json.flag;
                    $this.fileserverid = json.fileserverid;
                } else {
                    flag = 1;
                }
            } catch (e) {
                flag = 1;

            } finally {
                if (flag == 0) {
                    $this.status = $this.STATUS_UPLOADING;
                } else {
                    $this.status = $this.STATUS_ERROR;
                }

                try {
                    if ($this.process) {
                        $this.process($this.filestart + $this.blob, $this.message);
                    }
                } catch (e) {
                }

                $this.send();
            }
        }

        /**
        * 上传开始
        */
        $this.start = function () {
            $this.isPause = false;
            $this.status = $this.STATUS_PENDING;
            // check
            if (!$this.url) {
                $this.status = $this.STATUS_ERROR;
                $this.message = "url is null!"
            } else if (!$this.fileid) {
                $this.status = $this.STATUS_ERROR;
                $this.message = "fileid is null!"
            } else if (!$this.checkExtend()) {
                $this.status = $this.STATUS_ERROR;
                $this.message = "file type error!"
            }
            $this.send();
        }

        $this.isPause = false;
        /**
        * 暂停
        */
        $this.pause = function () {
            $this.isPause = true;
        }
        /**
        * 断点续传
        */
        $this.go = function () {
            if ($this.status == $this.STATUS_COMPLETE) {
                $this.isPause = true;
            }
            if ($this.isPause) {
                $this.isPause = false;
                if ($this.status == $this.STATUS_PENDING) {
                    $this.start();
                } else {
                    $this.status = $this.STATUS_UPLOADING;
                    $this.send();
                }
            }
        }
        /**
        * 数据发送
        */
        $this.send = function () {
            if ($this.isPause) {
                // 已暂停
                return;
            }
            switch ($this.status) {
                case $this.STATUS_SUCCESS:
                    {
                        try {
                            if ($this.success) {
                                $this.success($this.message);
                            }
                        } catch (e) {
                            console.log("success:");
                            console.log(e);
                        } finally {
                            $this.status = $this.STATUS_COMPLETE;
                            $this.send();
                            return;
                        }
                    }
                    break;
                case $this.STATUS_ERROR:
                    {
                        if ($this.RETRY_TIMES < $this.RETRY_MAX_TIMES) {
                            $this.RETRY_TIMES++;
                            $this.status == $this.STATUS_UPLOADING;
                        } else {
                            try {
                                if ($this.error) {
                                    $this.error($this.message);
                                }
                            } catch (e) {
                                console.log("error:");
                                console.log(e);
                            } finally {
                                $this.status = $this.STATUS_COMPLETE;
                                $this.send();
                                return;
                            }
                        }
                    }
                    break;
                case $this.STATUS_PENDING:
                    {
                        $this.reset();
                    }
                    break;
                case $this.STATUS_UPLOADING:
                    {
                        $this.filestart = $this.fileblob + $this.filestart;
                    }
                    break;
                case $this.STATUS_COMPLETE:
                    {
                        try {
                            if ($this.complete) {
                                $this.complete();
                            }
                        } catch (e) {
                            console.log("complete:");
                            console.log(e);
                        } finally {
                            // 结束
                            return;
                        }
                    }
                    break;
            }

            // 封装数据
            var data = new FormData();
            // fieldname:, //字段名称
            // filename:, //文件名称
            // filesize, //文件总大小
            // uolpadstatus, //当前起始位置
            // filetempname, //当前起始位置
            $this.blob = $this.getReadBlob();
            if ($this.blob <= 0) {
                // 上传完成
                $this.status = $this.STATUS_SUCCESS;
                $this.send();
                return;
            }
            var blobdata = $this.blobSlice($this.blob);
            if (blobdata == null) {
                // 上传完成
                $this.status = $this.STATUS_SUCCESS;
                $this.send();
                return;
            }
            data.append("fieldname", $this.fileid);
            data.append("filename", $this.filename);
            data.append("filesize", $this.filemaxsize);
            data.append("filestart", $this.filestart);
            data.append("fileblob", $this.blob);
            data.append("fileserverid", $this.fileserverid);
            data.append("blob", blobdata);
            if ($this.sign) {
                var reader = new FileReader();
                //当读取操作成功完成时调用.
                reader.onload = function (evt) {
                    if (evt.target.readyState == FileReader.DONE) {
                        var d = new Uint8Array(evt.currentTarget.result);
                        var sign = $this.sign(d);
                        data.append("sign", sign);
                        //console.log("sign:" + sign);
                        $this.xhrSend(data);
                    }
                }
                reader.readAsArrayBuffer(blobdata);
            } else {
                $this.xhrSend(data);
            }
        }
        $this.xhrSend = function (data) {
            $this.xhr = createXHR();
            $this.xhr.open($this.method, $this.url);
            $this.xhr.addEventListener('load', $this.handler, false);
            $this.xhr.addEventListener('error', $this.handler, false);
            $this.xhr.send(data);
        }
        /**
        * 获取需要发送的数据
        */
        $this.blobSlice = function (blob) {
            var end = $this.filestart + blob;
            if ($this.file.slice) {
                return $this.file.slice($this.filestart, end, {
                    type: "text/plain"
                });
            }
            // 兼容firefox
            else if ($this.file.mozSlice) {
                return $this.file.mozSlice($this.filestart, end, {
                    type: "text/plain"
                });
            }
            // 兼容webkit
            else if ($this.file.webkitSlice) {
                return $this.file.webkitSlice($this.filestart, end, {
                    type: "text/plain"
                });
            }
            return null;
        }

        // 状态枚举值
        /**
        * 执行完成
        */
        $this.STATUS_COMPLETE = 0;
        /**
        * 未开始
        */
        $this.STATUS_PENDING = 1;
        /**
        * 正在上传
        */
        $this.STATUS_UPLOADING = 2;
        /**
        * 上传成功
        */
        $this.STATUS_SUCCESS = 3;
        /**
        * 上传错误
        */
        $this.STATUS_ERROR = 4;

        /**
        * 当前重试次数
        */
        $this.RETRY_TIMES = 0;

        return this;
    }

    /**
    * 创建 XMLHttpRequest
    */
    function createXHR() {
        if (typeof XMLHttpRequest != "undefined") {
            return new XMLHttpRequest();
        }
        if (typeof ActiveXobject == "undefined") {
            throw new Error(" not support ");
        }
        // 判断是否为 IE
        if (typeof arguments.callee.activeString != "string") {
            var versions = ["MSXML2.XMLHttp.6.0", "MSXML2.XMLHttp3.0",
					"MSXML2.XMLHttp"], i, len;

            for (var i = 0; i < versions.length; i++) {
                try {
                    new ActiveXobject(versions[i]);
                    arguments.callee.activeString = versions[i];
                    break;
                } catch (ex) {
                    // no action
                }
            }
            ;
        }
        return new ActiveXobject(arguments.callee.activeString);
    }
    window.amFileUpload = amFileUpload;
})();
