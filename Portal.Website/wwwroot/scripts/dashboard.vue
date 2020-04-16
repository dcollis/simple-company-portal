<template>
    <div class="profile-page">
        <navbar page="dashboard" />

        <div class="page-header header-filter" data-parallax="true" style="background-color: #24145d">
            <div class="container">
                <div class="row">
                    <div class="col-md-8 ml-auto mr-auto">
                        <div class="brand text-center">
                            <h1>Our Merit</h1>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div style="margin-left: 20px; margin-right: 20px;" class="bg-transparent">
            <div class="bg-transparent">
                <div class="section bg-transparent" style="padding-left: 20px; padding-right: 20px;">
                    <div class="row">
                        <div class="col-md-8 newsCard">

                            <div class="card">
                                <div class="card-body" id="newsFeed">
                                    <div class="text-center">
                                        <h4 class="card-title">{{ $t("news") }}</h4>
                                        <h6 class="card-subtitle mb-2 text-muted">{{ $t("latest") }}</h6>
                                    </div>
                                    <newsWidget page="1" pagesize="4" dark="true"></newsWidget>
                                </div>
                            </div>

                        </div>
                        <div class="col-md-4" >
                            <div class="row" v-if="false">
                                <documentWidget></documentWidget>
                            </div>
                            <div class="row">
                                <calendarWidget></calendarWidget>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <portalFooter />
        </div>
    </div>
</template>

<script type="text/javascript">
    import navbar from './components/navbar.vue';
    import portalFooter from './components/footer.vue';
    import newsWidget from './components/widgets/newsWidget.vue';
    import loadingWidget from './components/loading.vue';
    import documentWidget from './components/widgets/dashboard/latestDocumentWidget.vue';
    import calendarWidget from './components/widgets/dashboard/latestCalendar.vue';

    export default {
        components: {
            navbar,
            newsWidget,
            portalFooter,
            loadingWidget,
            documentWidget,
            calendarWidget
        }
    }
</script>

<style>
    @media screen and (min-width: 767px) {
        .newsCard {
            padding-left: 0px;
        }
    }
    @media screen and (max-width: 767px) {
        .newsCard {
            padding: 0px;
        }
    }
</style>
