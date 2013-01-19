var MonsterApp;
(function (MonsterApp) {
    (function (Utils) {
        function isObjectPresent(obj) {
            return typeof obj !== 'undefined' && obj !== null;
        }
        Utils.isObjectPresent = isObjectPresent;
    })(MonsterApp.Utils || (MonsterApp.Utils = {}));
    var Utils = MonsterApp.Utils;
})(MonsterApp || (MonsterApp = {}));
