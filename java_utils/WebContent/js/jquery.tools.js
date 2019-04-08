/**
定义工具扩展至jQuery
@author am
@date 2017-10-25
@update 
 
*/
(function (w) {
    var Tool = function () {

        /**
            数字转十六进制字符串
        */
        this.toHexString = function (val) {
            return (parseInt(val, 10) >>> 0).toString(16);
        }


        return this;
    }
    w.tools = new Tool();
})(window.jQuery);
