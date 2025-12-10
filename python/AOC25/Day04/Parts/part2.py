from pathlib import Path

import Parts.Util.handlers as hs


class Part2:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = [list(line) for line in f.readlines()]

        self.x_max = len(self.input_data)
        self.y_max = len(self.input_data[0]) - 1

    def get_day_solution(self):
        total_processed = 0

        while True:
            accessible = hs.handle_grid(
                self.input_data, self.x_max, self.y_max, remove=True
            )
            amount = len(accessible)

            if amount == 0:
                # we stop when we can't process more paper rolls
                break
            else:
                # process the accessible paper rolls
                total_processed += amount

        return str(total_processed)
