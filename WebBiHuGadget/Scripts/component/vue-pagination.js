var pageComponent = Vue.extend({
    template: `<div class="ui floated pagination menu">
                    <a class="icon item" :class="{\'disabled\':curPage==1}" v-on:click.stop="goPage(curPage==1?curPage:curPage-1)">
                        <i class="left chevron icon"></i>
                    </a>
                    <a class="item" v-for="(page,index) in selectPage" :class="{'p-active':page==curPage}" v-on:click.stop="goPage(page)">
                        <template v-if="page">{{page}}</template>
                        <template v-else="page" >···</template>
                    </a>
                    <a class="icon item" :class="{\'disabled\':curPage==pages}" v-on:click.stop="goPage(curPage==pages?curPage:curPage+1)">
                        <i class="right chevron icon"></i>
                    </a>
                </div>`,
    props: {
        pages: {
            type: Number,
            default: 1
        },
        current: {
            type: Number,
            default: 1
        }
    },
    data() {
        return {
            curPage: 1
        }
    },
    computed: {
        selectPage() {
            let pageNum = this.pages;
            let index = this.curPage;
            let arr = [];
            $('.pagination>.item').removeClass('active');
            if (pageNum <= 5) {
                for (var i = 1; i <= pageNum; i++) {
                    arr.push(i);
                }
                return arr;
            }
            if (index <= 2) return [1, 2, 3, 0, pageNum];
            if (index >= pageNum - 1) return [1, 0, pageNum - 2, pageNum - 1, pageNum];
            if (index === 3) return [1, 2, 3, 4, 0, pageNum];
            if (index === pageNum - 2) return [1, 0, pageNum - 3, pageNum - 2, pageNum - 1, pageNum];
            return [1, 0, index - 1, index, index + 1, 0, pageNum];
        }
    },
    methods: {
        goPage: function (page) {
            if (page == 0)
                return;
            if (page != this.curPage) {
                $('.pagination>.item').removeClass('active');
                this.curPage = page;
                this.$emit('navpage', this.curPage);
                $('.pagination>.item').removeClass('active');
            } else {
                console.log('Already in the current page');
            }
        }
    },
    watch: {
        'curPage': function () {
            $('.pagination>.item').removeClass('active');
        }
    }
});
Vue.component('pagination', pageComponent);