import { defineStore } from 'pinia';
import { useRouter } from 'vue-router';

export const usePageNameStore = defineStore('pageName', {
    state: () => ({
        pageName: null
    }),
    actions: {
        async setPageName() {
            const router = useRouter();
            
            let pageName = "";
  
            const path = router.currentRoute.value.path;

            switch(path){
                case "/":
                    pageName = "Oversikt";
                    break;
                case "/create":
                    pageName = "Ny sak";
                    break;
                case "/tickets":
                    pageName = "Mine saker";
                    break;
            }

            this.pageName = pageName;
        }
    }
});