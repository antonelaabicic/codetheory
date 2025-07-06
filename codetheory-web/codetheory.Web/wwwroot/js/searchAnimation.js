(() => {
    function sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    const boxWidth = 55;
    const spacing = 5;
    const leftMargin = 10;
    const boxCount = 16;
    const containerWidth = (boxWidth + spacing) * boxCount + leftMargin + 10;

    function generateSortedArray() {
        let arr = Array.from({ length: boxCount }, () => Math.floor(Math.random() * 900 + 100));
        return arr.sort((a, b) => a - b);
    }
    window.initSearchKonva = function () {
        const stage = new Konva.Stage({
            container: 'konvaContainer',
            width: containerWidth,
            height: 130
        });

        const layer = new Konva.Layer();
        stage.add(layer);

        const numbers = generateSortedArray();

        window._searchState = {
            stage,
            layer,
            numbers,
            boxes: []
        };

        renderBoxes(numbers, layer);
    };
    function renderBoxes(numbers, layer) {
        layer.destroyChildren();
        const boxes = [];

        numbers.forEach((num, index) => {
            const group = new Konva.Group({
                x: leftMargin + index * (boxWidth + spacing),
                y: 20
            });

            const rect = new Konva.Rect({
                width: boxWidth,
                height: 50,
                stroke: 'black',
                strokeWidth: 1,
                fill: 'white'
            });

            const text = new Konva.Text({
                text: num.toString(),
                fontSize: 16,
                width: boxWidth,
                height: 50,
                align: 'center',
                verticalAlign: 'middle'
            });

            group.add(rect);
            group.add(text);
            layer.add(group);
            boxes.push({ group, rect, text });
        });

        layer.draw();

        window._searchState.numbers = numbers;
        window._searchState.boxes = boxes;
        window._searchState.layer = layer;
    }

    window.clearSearchBoard = function () {
        const { boxes, layer } = window._searchState;
        boxes.forEach(b => b.rect.fill('white'));
        layer.draw();
    };

    window.randomizeSearchArray = function () {
        const { stage } = window._searchState;
        const layer = new Konva.Layer();
        stage.destroyChildren();
        stage.add(layer);

        const numbers = generateSortedArray();
        renderBoxes(numbers, layer);
    };

    window.animateSearch = async function (algorithm, target) {
        const { boxes, numbers, layer } = window._searchState;
        const t = parseInt(target);

        function highlight(index, color) {
            boxes[index].rect.fill(color);
            layer.draw();
        }

        boxes.forEach(b => b.rect.fill('white'));
        layer.draw();

        if (algorithm === "Linear") {
            for (let i = 0; i < numbers.length; i++) {
                highlight(i, 'yellow');
                await sleep(500);

                if (numbers[i] === t) {
                    highlight(i, 'green');
                    return;
                } else {
                    highlight(i, 'red');
                    await sleep(200);
                }
            }
            alert("Number not found.");
        }

        if (algorithm === "Binary") {
            let left = 0;
            let right = numbers.length - 1;

            while (left <= right) {
                let mid = Math.floor((left + right) / 2);
                highlight(mid, 'yellow');
                await sleep(600);

                if (numbers[mid] === t) {
                    highlight(mid, 'green');
                    return;
                } else {
                    highlight(mid, 'red');
                    await sleep(300);
                    if (numbers[mid] < t) {
                        left = mid + 1;
                    } else {
                        right = mid - 1;
                    }
                }
            }

            alert("Number not found.");
        }
    };
})();