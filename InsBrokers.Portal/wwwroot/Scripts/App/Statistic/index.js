/// <reference path="../../Libs/jquery-3.1.1.min.js" />
var randomColor = new (function () {
    let colors = [
        { color: '#55efc4', bColor: '#009e72' },//light green blue
        { color: '#a29bfe', bColor: '#534acb' },//shy moment
        { color: '#00b894', bColor: '#006753' },//mint leaf
        { color: '#74b9ff', bColor: '#4777a9' },//green darner tail
        { color: '#81ecec', bColor: '#76d2d2' },//faded poster
        { color: '#0984e3', bColor: '#066cbc' },//election blue
        { color: '#00cec9', bColor: '#019f9b' },//robin's edge blue
        { color: '#6c5ce7', bColor: '#483bb1' },//exodus fruit
        { color: '#ffeaa7', bColor: '#ffc91d' },//sour lemon
        { color: '#ff7675', bColor: '#d44342' },//pink glamour
        { color: '#fab1a0', bColor: '#f97f63' },//first date
        { color: '#fd79a8', bColor: '#ea6293' },//pico 8 pink
        { color: '#e17055', bColor: '#ce634a' },//orange evill
        { color: '#fdcb6e', bColor: '#ecb54c' },//bright yarrow
        { color: '#e84393', bColor: '#c33b7d' },//prunus avium
        { color: '#fdcb6e', bColor: '#ecb54c' },//bright yarrow
        { color: '#d63031', bColor: '#b82b2c' },//chi gong
    ];
    let ins = function () { };
    ins.prototype.get = function (count) {
        let temp = JSON.parse(JSON.stringify(colors));
        let rep = [];
        let max = colors.length;
        if (max >= count) {
            for (var i = 0; i < count; i++) {
                let item = temp[Math.floor(Math.random() * temp.length)];
                rep.push(item);
                temp.splice(temp.findIndex(x => x.color === item.color && x.bColor === item.bColor), 1);
            }
        }
        else {

            for (var i = 0; i < count; i++) {
                rep.push(colors[Math.floor(Math.random() * max)]);
            }
        }

        return rep;
    };
    return ins;

}());

$(document).ready(function () {
    let rand = randomColor.get(2);
    let chtUserData = {
        label: 'تعداد کاربران',
        backgroundColor: rand[0].color,
        borderColor: rand[0].bColor,
        borderWidth: 1,
        data: Object.values(userInDays)
    };

    initBarChart($('#user_in_days'), Object.keys(userInDays), chtUserData);

    let chtLossData = {
        label: 'تعداد ثبت هزینه',
        backgroundColor: rand[1].color,
        borderColor: rand[1].bColor,
        borderWidth: 1,
        data: Object.values(lossInDays)
    };

    initBarChart($('#loss_in_days'), Object.keys(lossInDays), chtLossData);

});

var initBarChart = function ($elm,lbls,data) {
    var config = {
        type: 'bar',
        data: {
            labels: lbls,
            datasets: [data]
        },
        options: {
            maintainAspectRatio: false,
            responsive: true,
            //legend: { display: false },
            hover: {
                mode: 'nearest',
                intersect: true
            },
            scales: {
                yAxes: [{ ticks: { min: 0, beginAtZero: true, fontFamily: 'Iransans' } }],
                xAxes: [{ ticks: { fontFamily: 'Iransans' } }]
            },
            tooltips : {
                position: 'nearest',
                intersect: false,
                //borderWidth: 2,
                bodyFontFamily: 'Iransans',
                titleFontFamily: 'Iransans',
                xPadding: 10,
                yPadding: 10,
                custom: function (tooltip) {
                    if (!tooltip) return;
                    tooltip.displayColors = false;
                }
            },
            legend : {
                display: true,
                position: 'bottom',
                labels: {
                    fontFamily: 'iransans',
                    //generateLabels: function (chart) {
                    //    return chart.data.datasets.map(function (data, i) {
                    //        let lblVal = data.label;
                    //        if (info.legends) lblVal = info.legends[i];
                    //        else if (dictionary[data.label]) lblVal = dictionary[data.label];
                    //        return {
                    //            // And finally : 
                    //            text: lblVal + ':',
                    //            fillStyle: data.backgroundColor,
                    //            //strokeStyle: stroke,
                    //            //lineWidth: bw,
                    //            //hidden: isNaN(ds.data[i]) || meta.data[i].hidden,
                    //            index: i
                    //        };
                    //    });
                    //}
                }
            }
        }
    };


    let cht = new Chart($elm, config);
}