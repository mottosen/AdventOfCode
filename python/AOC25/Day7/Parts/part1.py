import re
from pathlib import Path


class Part1:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = [list(line) for line in f.readlines()]

        self.input_length = len(self.input_data)

    def get_day_solution(self):
        # we draw the beams, and count when we split
        self.input_data[1][self.input_data[0].index("S")] = "|"
        total_splits = 0

        for i, line in enumerate(self.input_data):
            if i == 0 or i >= self.input_length - 1:
                continue

            line = "".join(line)
            next_line = self.input_data[i + 1]

            beams = re.finditer(r"\|", line)
            for beam in beams:
                beam_i = beam.start(0)

                if next_line[beam_i] == "^":
                    # if the beam meets a split
                    total_splits += 1
                    next_line[beam_i - 1] = "|"
                    next_line[beam_i + 1] = "|"
                else:
                    # if the beam continues
                    next_line[beam_i] = "|"

        return str(total_splits)
