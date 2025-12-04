from pathlib import Path

import Parts.Util.handlers as hs


class Part1:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = [list(line) for line in f.readlines()]

        self.x_max = len(self.input_data)
        self.y_max = len(self.input_data[0]) - 1

    def get_day_solution(self):
        return str(len(hs.handle_grid(self.input_data, self.x_max, self.y_max)))
