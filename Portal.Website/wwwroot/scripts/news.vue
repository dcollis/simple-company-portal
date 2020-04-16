<template>
    <div>
        <navbar page="news"></navbar>

        <div class="container">
            <newsWidget page="1" pagesize="10" loadOnScroll="true"></newsWidget>
        </div>
        <portalFooter />
        <addNewsButton></addNewsButton>
    </div>
</template>

<script>
    import navbar from './components/navbar.vue';
    import portalFooter from './components/footer.vue';
    import newsWidget from './components/widgets/newsWidget.vue';
    import addNewsButton from './components/widgets/addNewsWidget.vue';

    export default {
        components: {
            navbar,
            portalFooter,
            newsWidget,
            addNewsButton
        }
    }
</script>