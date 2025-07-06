(() => {
    const barCount = 30;
    const containerHeight = 250;
    const barWidth = 26;
    const spacing = 5;
    const containerPadding = 10;

    let sortState = {
        stage: null,
        layer: null,
        bars: [],
        values: [],
        labels: [],
        paused: false,
        speed: 250,
        onFinish: null 
    };

    const sleep = (ms) => new Promise(resolve => setTimeout(resolve, ms));

    async function controlledSleep() {
        const interval = 10;
        let elapsed = 0;
        while (elapsed < sortState.speed) {
            while (sortState.paused) await sleep(interval);
            await sleep(interval);
            elapsed += interval;
        }
    }

    function generateRandomArray() {
        return Array.from({ length: barCount }, () => Math.floor(Math.random() * 150) + 1);
    }

    function renderBars(values, layer) {
        layer.destroyChildren();
        const bars = [];
        const labels = [];

        values.forEach((value, index) => {
            const x = containerPadding + index * (barWidth + spacing);
            const y = containerHeight - value - 20;

            const rect = new Konva.Rect({
                x, y,
                width: barWidth,
                height: value,
                fill: 'steelblue',
                stroke: 'black',
                strokeWidth: 1
            });

            const label = new Konva.Text({
                x,
                y: y - 20,
                width: barWidth,
                text: value.toString(),
                fontSize: 14,
                fill: 'black',
                align: 'center'
            });

            layer.add(rect);
            layer.add(label);
            bars.push(rect);
            labels.push(label);
        });

        layer.draw();
        Object.assign(sortState, { bars, values, labels });
    }

    function updateBar(i, value) {
        const { bars, labels } = sortState;
        if (!bars[i] || !labels[i]) return;
        bars[i].height(value);
        bars[i].y(containerHeight - value - 20);
        labels[i].text(value);
        labels[i].y(bars[i].y() - 20);
        labels[i].x(bars[i].x());
    }

    window.initSortKonva = function () {
        const container = document.getElementById('sortKonvaContainer');
        const width = container.offsetWidth;

        const stage = new Konva.Stage({
            container: 'sortKonvaContainer',
            width,
            height: containerHeight
        });

        const layer = new Konva.Layer();
        stage.add(layer);
        const values = generateRandomArray();
        renderBars(values, layer);
        Object.assign(sortState, { stage, layer });
    };

    window.randomizeSortArray = () => renderBars(generateRandomArray(), sortState.layer);

    window.clearSortBoard = function () {
        sortState.bars.forEach(b => b.fill('steelblue'));
        sortState.labels.forEach((l, i) => l.text(sortState.values[i].toString()));
        sortState.layer.draw();
    };

    window.pauseSort = () => sortState.paused = true;
    window.resumeSort = () => sortState.paused = false;
    window.setSortSpeed = (ms) => sortState.speed = ms;

    window.setSortFinishCallback = (dotnetHelper) => {
        sortState.onFinish = () => dotnetHelper.invokeMethodAsync('OnSortFinished');
    };

    window.animateSort = async function (algorithmId) {
        const { bars, labels, layer } = sortState;
        let values = [...sortState.values];

        function swap(i, j) {
            [values[i], values[j]] = [values[j], values[i]];
            const tempX = bars[i].x();
            bars[i].x(bars[j].x());
            bars[j].x(tempX);

            [bars[i], bars[j]] = [bars[j], bars[i]];
            [labels[i], labels[j]] = [labels[j], labels[i]];
            updateBar(i, values[i]);
            updateBar(j, values[j]);
            layer.draw();
        }

        function highlight(i, color) {
            if (bars[i]) bars[i].fill(color);
            layer.draw();
        }

        function resetColor(i) {
            if (bars[i]) bars[i].fill('steelblue');
            layer.draw();
        }

        async function bubbleSort() {
            for (let i = 0; i < values.length - 1; i++) {
                for (let j = 0; j < values.length - i - 1; j++) {
                    highlight(j, 'yellow');
                    highlight(j + 1, 'yellow');
                    await controlledSleep();
                    if (values[j] > values[j + 1]) {
                        swap(j, j + 1);
                    }
                    resetColor(j);
                    resetColor(j + 1);
                }
            }
        }

        async function selectionSort() {
            for (let i = 0; i < values.length; i++) {
                let min = i;
                highlight(min, 'green');
                for (let j = i + 1; j < values.length; j++) {
                    highlight(j, 'yellow');
                    await controlledSleep();
                    if (values[j] < values[min]) {
                        resetColor(min);
                        min = j;
                        highlight(min, 'green');
                    } else {
                        resetColor(j);
                    }
                }
                if (i !== min) {
                    swap(i, min);
                }
                resetColor(i);
                resetColor(min);
            }
        }

        async function insertionSort() {
            for (let i = 1; i < values.length; i++) {
                let j = i;
                while (j > 0 && values[j] < values[j - 1]) {
                    highlight(j, 'yellow');
                    highlight(j - 1, 'yellow');
                    await controlledSleep();
                    swap(j, j - 1);
                    resetColor(j);
                    resetColor(j - 1);
                    j--;
                }
            }
        }

        async function mergeSort(start = 0, end = values.length - 1) {
            if (start >= end) return;

            const mid = Math.floor((start + end) / 2);
            await mergeSort(start, mid);
            await mergeSort(mid + 1, end);

            const merged = [];
            let i = start, j = mid + 1;
            const highlighted = new Set();

            while (i <= mid && j <= end) {
                highlight(i, 'yellow');
                highlight(j, 'yellow');
                highlighted.add(i);
                highlighted.add(j);

                await controlledSleep();

                if (values[i] < values[j]) {
                    merged.push(values[i++]);
                } else {
                    merged.push(values[j++]);
                }
            }

            while (i <= mid) {
                highlight(i, 'yellow');
                highlighted.add(i);
                await controlledSleep();
                merged.push(values[i++]);
            }

            while (j <= end) {
                highlight(j, 'yellow');
                highlighted.add(j);
                await controlledSleep();
                merged.push(values[j++]);
            }

            for (let k = 0; k < merged.length; k++) {
                const idx = start + k;
                values[idx] = merged[k];
                updateBar(idx, merged[k]);
            }

            highlighted.forEach(resetColor);
            layer.draw();
        }
        async function quickSort(low = 0, high = values.length - 1) {
            if (low < high) {
                const pi = await partition(low, high);
                await quickSort(low, pi - 1);
                await quickSort(pi + 1, high);
            }
        }

        async function partition(low, high) {
            const pivot = values[low];
            highlight(low, 'red');
            let i = low + 1;

            const toReset = [low];

            for (let j = low + 1; j <= high; j++) {
                highlight(j, 'yellow');
                toReset.push(j);

                await controlledSleep();

                if (values[j] < pivot) {
                    swap(i, j);
                    i++;
                }
            }

            swap(low, i - 1);
            toReset.push(i - 1);

            toReset.forEach(idx => resetColor(idx));

            return i - 1;
        }



        switch (algorithmId) {
            case 1: await bubbleSort(); break;
            case 2: await selectionSort(); break;
            case 3: await insertionSort(); break;
            case 4: await mergeSort(); break;
            case 5: await quickSort(); break;
        }

        sortState.values = values;
        if (typeof sortState.onFinish === 'function') {
            sortState.onFinish();
        }
    };
})();
