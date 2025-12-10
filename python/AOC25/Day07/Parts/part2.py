import re
from pathlib import Path


class Part2:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = [list(line) for line in f.readlines()]

        self.input_length = len(self.input_data)

    def insert_beam(self, dst, num):
        # we have to merge overlapping timelines
        if dst == ".":
            return num
        else:
            return str(int(dst) + int(num))

    def get_day_solution(self):
        # we draw counted timelines
        self.input_data[1][self.input_data[0].index("S")] = "1"

        for i, line in enumerate(self.input_data):
            if i == 0 or i >= self.input_length - 1:
                continue

            next_line = self.input_data[i + 1]

            for beam_i, beam in enumerate(line):
                if re.match(r"\d+", beam):
                    if next_line[beam_i] == "^":
                        # current timeline(s) should split
                        next_line[beam_i - 1] = self.insert_beam(
                            next_line[beam_i - 1], beam
                        )
                        next_line[beam_i + 1] = self.insert_beam(
                            next_line[beam_i + 1], beam
                        )
                    else:
                        # current timeline(s) continues
                        next_line[beam_i] = self.insert_beam(next_line[beam_i], beam)

        # last line will now contain all timelines to be summed
        timelines = 0
        for pos in self.input_data[-1]:
            if time := re.match(r"\d+", pos):
                timelines += int(time.group(0))

        return str(timelines)
