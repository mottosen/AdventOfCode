import multiprocessing as mp
from pathlib import Path

import Parts.util.handlers as handlers


class Part1:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = [handlers.line_2_binary(line) for line in f.readlines()]

        self.worker_pool = mp.Pool()

    def get_day_solution(self):
        return str(
            sum(  # summing button presses
                self.worker_pool.map(  # handle each light problem in parallel
                    handlers.turn_off_lights, self.input_data
                )
            )
        )
