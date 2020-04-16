<template>
    <div>
        <navbar></navbar>

        <div class="container card" style="display: flex; flex-flow: column; height: 100%;">
            <div style="height: 100px;">
                <h3 class="text-center" style="vertical-align: middle">Something went wrong</h3>
            </div>
            <div style="flex-grow: 1;" id="errorMessage">
                <p class="text-center capitalize">{{ errorMessage }}</p>
            </div>
        </div>

        <portalFooter></portalFooter>
    </div>
</template>

<script>
    import navbar from './components/navbar.vue';
    import portalFooter from './components/footer.vue';

    export default {
        data() {
            this.getErrorMessage();

            return {
                errorMessage: ''
            };
        },
        methods: {
            getErrorMessage: function () {
                this.errorMessage = decodeURI(window.location.hash.substr(1));
                window.location.hash = '';
            }
        },
        components: {
            navbar,
            portalFooter,
        }
    }
</script>