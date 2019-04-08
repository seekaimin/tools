(function (window, $) {
    var Pagings = function () {
    };
    Pagings.map = Array.prototype.map;
    Pagings.init = function (query) {
        var tempq = new PagingQuery();
        tempq.init(query);
        this.map[tempq.id] = tempq;
        tempq.createDom();
        return this;
    };
    Pagings.get = function (id) {
        return this.map[id];
    };
    //Pagings.query = Pagings.prototype.query;
    //Pagings.init = Pagings.prototype.init;
    window.Pagings = Pagings;
})(window, jQuery);


/*index:1~n*/
function PagingQuery() {
    this.id = "";
    this.index = 0;
    this.size = 20;
    this.count = 0;
    this.max_display_count = 5;
    this.action = undefined;
    this.init = function (query) {
        this.id = query.id;
        this.index = query.index;
        if (query.size)
            this.size = query.size;
        if (query.count)
            this.count = query.count;
        if (query.action)
            this.action = query.action;
        if (query.max_display_count)
            this.max_display_count = query.max_display_count;
    };
    this.totalPages = function () {
        var tal = Math.floor((this.count - 1) / this.size);
        if (tal < 0) {
            tal = 0;
        }
        return tal + 1;
    };
    this.createDom = function () {
        this.ckeck();
        var dom = $("<ul></ul>");
        var first = "<li class='btn' name ='first'><a href='#'>|&lt;</a></li>";
        var pre = "<li class='btn' name='pre'><a href='#'>&lt;</a></li>";
        var next = "<li class='btn' name='next'><a href='#'>&gt;</a></li>";
        var last = "<li class='btn' name='last'><a href='#'>&gt;|</a></li>";
        dom.append(first);
        dom.append(pre);

        var tal = this.totalPages();
        var inputindex = $("<li class='paging active'><input type='text' name='paging_input_value' value = '" + this.index + "' /></li>");
        var char = "<li class='btn'><span>/</span></li>";
        var tal = "<li class='btn active' name='total'><span>" + tal + "</span></li>";

        dom.append(inputindex);
        dom.append(char);
        dom.append(tal);

        dom.append(next);
        dom.append(last);
        var cbtn = $("<li class='paging'><a href='#' name='btnGo'>GO</a></li>");


        dom.append(cbtn);
        this.registevents(this.id, dom);
        $("#" + this.id).html("");
        $("#" + this.id).html(dom);
    };
    this.setCount = function (c) {
        this.count = c;
        this.createDom();
    };
    this.ckeck = function () {
        if (this.index < 1) {
            this.index = 1;
        } else if (this.index > this.totalPages()) {
            this.index = this.totalPages();
        }
    };

    this.registevents = function (id, dom) {
        var first = dom.find("[name=first]");
        var pre = dom.find("[name=pre]");
        var next = dom.find("[name=next]");
        var last = dom.find("[name=last]");
        var btn_index_value = dom.find("[name=paging_input_value]");
        var btn_go = dom.find("[name=btnGo]");
        first.click(function () {
            var p = Pagings.get(id);
            p.index = 1;
            searchByPaging(p);
        });
        pre.click(function () {
            var p = Pagings.get(id);
            p.index--;
            searchByPaging(p);
        });
        next.click(function () {
            var p = Pagings.get(id);
            p.index++;
            searchByPaging(p);
        });
        last.click(function () {
            var p = Pagings.get(id);
            p.index = p.totalPages();
            searchByPaging(p);
        });


        btn_go.click(function () {
            var p = Pagings.get(id);
            var v = Math.floor(btn_index_value.val());
            if (isNaN(v)) {
                v = 1;
            }
            p.index = v;
            searchByPaging(p);
         });
        btn_index_value.keydown(function (e) {
            if (!e) {
                e = window.event;
            }
            if ((e.keyCode || e.which) === 13) {
                btn_go.click();
            }
        });
        
    };

}
function searchByPaging(p) {
    var tal = p.totalPages();
    p.ckeck();
    p.action(p);
};