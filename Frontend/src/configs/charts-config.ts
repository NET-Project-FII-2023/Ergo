export const chartsConfig = {
  chart: {
    toolbar: {
      show: false,
    },
  },
  title: {
    show: "",
  },
  dataLabels: {
    enabled: false,
  },
  xaxis: {
    axisTicks: {
      show: false,
    },
    axisBorder: {
      show: false,
    },
    labels: {
      style: {
        colors: "#b4b1ba",
        fontSize: "13px",
        fontFamily: "inherit",
        fontWeight: 300,
      },
    },
  },
  yaxis: {
    labels: {
      style: {
        colors: "#b4b1ba",
        fontSize: "13px",
        fontFamily: "inherit",
        fontWeight: 300,
      },
      formatter: function(val:number) {
        return val.toFixed(0);
      }
    },
  },
  grid: {
    show: true,
    borderColor: "#352e42",
    strokeDashArray: 5,
    xaxis: {
      lines: {
        show: true,
      },
    },
    padding: {
      top: 5,
      right: 20,
    },
  },
  fill: {
    opacity: 0.8,
  },
  tooltip: {
    theme: "dark",
  },
  noData: {
    text: "No tasks due this week",
    align: 'center',
    verticalAlign: 'middle',
    offsetX: 0,
    offsetY: -15,
    style: {
      color: "#b4b1ba",
      fontSize: '20px',
    }
  }
};

export default chartsConfig;
