<template>
    <div>
        <div>
            <loadingAnimation v-if="loading"></loadingAnimation>
            <div v-else class="col card" v-bind:class="dark ? 'bg-dark' : ''" v-for="news in newsArticles" v-on:click="fetchArticle(news.newsArticleId)" data-toggle="modal" data-target="#selectedNewsArticle">
                <div class="card-body">
                    <h4 class="card-title">{{ news.title }}</h4>
                    <h6 class="card-subtitle mb-2 text-muted">{{ news.publishedString }}</h6>
                    <p class="card-text" v-html="news.body"></p>
                    <a href="#" class="card-subtitle mb-2 text-muted capitalize">{{ $t('read more') }}</a>
                </div>
            </div>
        </div>

        <div v-if="!loading" class="modal ps-child" id="selectedNewsArticle" tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLongTitle">{{ selectedArticle.title }}</h5>
                        <button type="button" class="close" data-dismiss="modal" :aria-label="$t('close')">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <h6 class="card-subtitle mb-2 text-muted">published on {{ selectedArticle.published.getDate() }}/{{ selectedArticle.published.getMonth() + 1 }}/{{ selectedArticle.published.getFullYear() }}</h6>

                        <div v-html="selectedArticle.body"></div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">{{ $t('close') }}</button>
                    </div>
                </div>
            </div>
        </div>
    </div>




</template>

<script>
    import loadingAnimation from '../loading.vue';
    import { get } from '../../utils/communication'; 

    export default {
        name: 'newsWidget',
        props: ['page', 'pagesize', 'dark', 'loadOnScroll'],
        data() {
            this.currentPage = Number(this.page);
            this.requiredPageSize = Number(this.pagesize);
            this.getTotalNumPages();
            this.getArticles(true);

            if (this.loadOnScroll) {
                window.onscroll = () => {
                    let bottomOfWindow = document.documentElement.scrollTop + window.innerHeight === document.documentElement.offsetHeight;

                    if (bottomOfWindow) {
                        this.loadMore();
                    }
                };
            }

            return {
                newsArticles: [],
                loading: true,
                selectedArticle: {
                    title: "Loading...",
                    body: "",
                    published: new Date()
                },
                totalNumPages: 10
            };
        },
        methods: {
            getArticles: function (showLoading) {
                var self = this;
                if (showLoading) {
                    self.loading = true;
                }
                get(
                    'api/news/getarticles?page=' + self.currentPage + '&pagesize=' + self.requiredPageSize,
                    function (response) {
                        for (var i = 0; i < response.length; i++) {
                            response[i].publishedDate = new Date(response[i].published);
                        }
                        self.newsArticles.push(...response);
                        self.loading = false;
                    });
            },
            fetchArticle: function (articleId) {
                var self = this;
                get('api/news/article?newsArticleId=' + articleId,
                    function (response) {
                        self.selectedArticle.title = response.title;
                        self.selectedArticle.body = response.body;
                        self.selectedArticle.published = new Date(response.published);

                        self.visible = true;
                    });
            },
            getTotalNumPages: function () {
                var self = this;
                get('api/news/getnumarticles',
                    function (response) {
                        self.totalNumPages = (response + self.requiredPageSize - 1) / self.requiredPageSize;
                    });
            },
            loadMore: function () {
                var self = this;
                if (self.currentPage < self.totalNumPages) {
                    self.currentPage++;
                    self.getArticles(false);
                }
            }
        },
        components: {
            loadingAnimation
        }
    }

</script>
