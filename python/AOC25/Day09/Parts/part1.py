from pathlib import Path


class Part1:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = f.readlines()

    def area(self, v1, v2):
        return (abs(v1[0] - v2[0]) + 1) * (abs(v1[1] - v2[1]) + 1)

    def get_day_solution(self):
        points = [tuple(map(int, line.split(","))) for line in self.input_data]
        largest_area = 0

        for i, v1 in enumerate(points[:-1]):
            for v2 in points[i + 1 :]:
                area = self.area(v1, v2)
                largest_area = max(largest_area, area)

        return str(largest_area)
