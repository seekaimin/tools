/**
扩展系统对象
@author am
@date 2017-10-25
@update 
 
*/
(function (w) {
    // 兼容IE8-，为Array原型添加indexOf方法；
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function (item) {
            var index = -1;
            for (var i = 0; i < this.length; i++) {
                if (this[i] === item) {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }
    // 为Array原型添加foreach方法
    if (!Array.prototype.foreach) {
        Array.prototype.foreach = function (callback) {
            for (var i = 0; i < this.length; i++) {
                var exit = callback(i, this[i]);
                if (exit) {
                    break;
                }
            }
        }
    }
    // 为Array原型添加removeAt方法
    if (!Array.prototype.removeAt) {
        Array.prototype.removeAt = function (index) {
            if (index != -1) {
                this.splice(index, 1)[0];
            }
            return this;
        }
    }
    
    /**
	* 
	*  对Date的扩展，将 Date 转化为指定格式的String 
	*  年(y)可以用 1-4 个占位符，毫秒(f)只能用 1 个占位符(是 1-3 位的数字) 
	*  月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，
	*  例子： 
	*  (new Date()).Format("yyyy-MM-dd hh:mm:ss.f") ==> 2006-07-02 08:09:04.423 
	*  (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
	* 调用： 
	* var time1 = new Date().Format("yyyy-MM-dd");
	* var time2 = new Date().Format("yyyy-MM-dd HH:mm:ss f");
	* 
	*/
    Date.prototype.format = function (fmt) {
        var o = {
            "M+": this.getMonth() + 1, // 月份 
            "d+": this.getDate(), // 日 
            "H+": this.getHours(), // 小时 
            "m+": this.getMinutes(), // 分 
            "s+": this.getSeconds(), // 秒 
            "q+": Math.floor((this.getMonth() + 3) / 3), // 季度 
            "f+": this.getMilliseconds() // 毫秒 
        };
        if (/(y+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(fmt)) {
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            }
        }
        return fmt;
    }
})(window);